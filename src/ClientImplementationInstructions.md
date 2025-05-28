# Amadeus.Net Client Implementation (Expert Guide)
Pattern for implementing "Amadeus for Devs" API endpoints in Amadeus.Net. Assumes C#13/NET9 & LanguageExt v5.

## Core Patterns
- **Context:** `AmadeusContext` exposes `Endpoint<Q, R>` properties (internally using `Lazy<T>` for optimized deferred initialization). Endpoints constructed via delegate.
- **Endpoint:** `readonly record struct Endpoint<Q, R>` has `Get(Q query) -> IO<Either<ErrorResponse, R>>`.
- **Query:** Immutable record/class implementing `IQuery` (`ToParams()` yields set `Seq<KVP>`). Static `From` (required), `With*` (optional). Optional query params: `Option<T>`.
- **Response Models:** Immutable records. Optionals: nullable types (e.g., `string?`).
- **Async:** All network ops are async (`InvokeAsync`). Error Handling: `Either<ErrorResponse, TResult>`.
- **Testing:** DI for `AmadeusContext`, call `context.Endpoint.Get(query).InvokeAsync(token)`, assert `Either`.

## Implementation Steps
1.  **Define Models:**
    *   **Instruction:** Based on the OpenAPI specification, implement all required Data Transfer Objects (DTOs).
    *   **Response Models:** Use sealed immutable C# records. Adhere to PascalCase naming. Represent optional fields with nullable C# types (e.g., `string?`, `int?`). Apply `[JsonPropertyName("api_field_name")]` attribute when C# property names need to differ from the JSON field names in the API response.
    *   **Query Models/Parameters:** Typically implemented as immutable C# records or classes (see Step 2).

2.  **Define Query Object:**
    *   **Instruction:** Create an immutable record or class for the endpoint's query parameters. This object encapsulates all possible inputs to the API call.
    *   **Interface:** It must implement the `IQuery` interface.
    *   **Serialization:** Implement `ToParams() -> Seq<KeyValuePair<string, string>>`. This method is crucial for serializing *only the set* (non-None) parameters into key-value pairs suitable for an HTTP query string or request body.
    *   **Construction:**
        *   Provide a static `From(...)` factory method for instantiating the query object with all *required* parameters.
        *   Offer composable `With*` instance methods for each *optional* parameter. These methods should return a new instance of the query object with the specified optional parameter set.
    *   **Optional Parameters:** Represent optional query parameters within the query object using `LanguageExt.Option<T>`. This clearly distinguishes unset parameters from those intentionally set with a null-equivalent value if the API supported it (though typically, unset optionals are just omitted from the request).

3.  **Register Endpoint in `AmadeusContext`:**
    *   **Instruction:** Make the new endpoint accessible through the central `AmadeusContext`.
    *   **Public Property:** Expose the endpoint as a public, read-only property of type `Endpoint<Q, R>` on `AmadeusContext`, where `Q` is your Query type and `R` is your Response type.
    *   **Lazy Initialization (Internal):** Internally, this public property should be backed by a `private readonly Lazy<Endpoint<Q, R>>` field. This is an optimization pattern to ensure the `Endpoint` (and its associated HTTP client configuration delegate) is instantiated only when first accessed.
    *   **Instantiation:** The `Lazy<T>` instance is constructed with a delegate. This delegate, when invoked, creates the `new Endpoint<Q, R>(...)`. The `Endpoint` constructor itself takes a delegate (e.g., `query => httpClient.Get<Q, R>(options, "/api/path", query)`) that defines how to execute the HTTP request using the provided `HttpClient` and `AmadeusOptions`.
    *   **Reference:** See the "Minimal Concrete Example" and existing endpoint registrations in `AmadeusContext.cs` for the precise pattern.

4.  **Implement xUnit Tests:**
    *   **Instruction:** Create comprehensive unit tests for the new endpoint to ensure its correctness and resilience.
    *   **Setup:** Use Dependency Injection (DI) to obtain an `AmadeusContext` instance. Configure necessary options (like `Amadeus:Host`) and secrets for the test environment.
    *   **Invocation:**
        *   Construct the query object using its static `From(...)` method and any relevant `With*` methods to simulate various request scenarios.
        *   Execute the endpoint call: `var result = await context.YourNewEndpoint.Get(query).InvokeAsync(cancellationToken);`.
    *   **Assertions:**
        *   Use `result.Match(...)` to handle both successful (`Right<YourResponse>`) and error (`Left<ErrorResponse>`) outcomes.
        *   For success cases, assert the structure and content of the response data (e.g., `Assert.NotNull(data.SomeField);`, `Assert.Equal("expected", data.SomeProperty);`).
        *   For error cases, assert that the correct type of `ErrorResponse` is returned under specific conditions (e.g., invalid input, simulated server errors if mockable).
    *   **Coverage:** Test various combinations of required and optional parameters.

**Global:** 100% OpenAPI coverage. Adhere to project conventions (see user rules/devlog). Consult the devlog or maintainers for questions.

---

## Minimal Concrete Example

Illustrates canonical pattern for new endpoint implementation and testing.

```csharp
// Namespace: Amadeus.Net.Clients.MyFeature

// Query
public sealed record MyQuery(
    string RequiredParameter,
    Option<string> OptionalParameter = default)
    : IQuery
{
    public static MyQuery From(string requiredParameter) => new(requiredParameter);
    public MyQuery WithOptionalParameter(string value) => this with { OptionalParameter = Prelude.Some(value) };
    public Seq<KeyValuePair<string, string>> ToParams() =>
        Prelude.Seq(
            Prelude.Some(KeyValuePair.Create("requiredKey", RequiredParameter)),
            OptionalParameter.Map(val => KeyValuePair.Create("optionalKey", val))
        ).Choose(option => option);
}

// Response
public sealed record MyResponse([property: JsonPropertyName("someField")] string? SomeField);

// AmadeusContext.cs (snippet)
private readonly Lazy<Endpoint<MyQuery, MyResponse>> myEndpoint =
    new(() => new Endpoint<MyQuery, MyResponse>(
        Get: query => httpClient.Get<MyQuery, MyResponse>(options, "/v1/myendpoint", query)));
public Endpoint<MyQuery, MyResponse> MyEndpoint => myEndpoint.Value;

// --- In YourTestsProject/Clients/MyNewFeatureClientTests.cs ---
// using Amadeus.Net.ApiContext;
// using Amadeus.Net.Clients.MyNewFeature; // Your new client's namespace
// using LanguageExt;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using System.Reflection;
// using Xunit;
public sealed class MyNewFeatureClientTests // Or a more descriptive name
{
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Amadeus:Host", "https://test.api.amadeus.com" }, // Or your test environment
            { "Amadeus:ClientVersion", "0.0.0" },
            { "Amadeus:ClientName", "Amadeus.Net.Tests" }, // Or your specific test client name (required by test server)
        }!)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true, false) // For API keys (required by test server)
        .Build();

    [Fact]
    public async Task MyNewEndpoint_WhenCalledWithValidQuery_ReturnsExpectedData()
    {
        var tkn = CancellationToken.None;

        var response = await Prelude.use(
            acquire: () => new ServiceCollection()
                .AddAmadeusContext(configuration) 
                .BuildServiceProvider(),
            release: provider => provider.Dispose())
            .Map(provider => provider.GetRequiredService<AmadeusContext>())
            .Bind(context => context.MyNewEndpoint.Get(
                MyQuery.From("someRequiredValue").WithOptionalParameter("optionalValue")))
            .InvokeAsync(tkn);

        _ = response.Match(
            Left: error => Assert.Fail($"Expected success, but got error: {error.Title} - {string.Join(',', error.Detail?.ToString() ?? string.Empty)}"),
            Right: data =>
            {
                Assert.NotNull(data);
                Assert.NotNull(data.SomeField);
                // Add more assertions based on expected response
                // e.g., Assert.Equal("expected value", data.SomeField);
            });
    }
}
```
---
For a full production example, refer to existing clients like the `namespace Amadeus.Net.Clients.AirlineCodeLookup` namespace.
