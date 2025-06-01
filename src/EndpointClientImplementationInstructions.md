# Amadeus.Net Endpoint Implementation Instructions
Pattern for implementing "Amadeus for Devs" API endpoints in Amadeus.Net. Assumes C#13/NET9 & LanguageExt v5.

## Core Patterns
- **Context:** `Context.AmadeusContext` exposes `Endpoint<Q, R>` properties. These are typically initialized by calling static `CreateEndpoint` factory methods from dedicated endpoint logic classes (e.g., `YourFeature.CreateEndpoint(...)` in `Amadeus.Net.Endpoints.YourFeature`). These factory methods, in turn, often use a central `Endpoint.Create<Q,R>(...)` factory (from `Amadeus.Net.Endpoints`).
- **Endpoint:** `readonly record struct Endpoint<Q, R>` (in `Amadeus.Net.Endpoints`) has `Get(Q query) -> IO<Either<ErrorResponse, R>>`.
- **Query:** Immutable record/class implementing `IQuery` (from `Amadeus.Net.Endpoints.Query`). `ToParams()` yields `Seq<QueryParameter>`. Static `From` (required), `With*` (optional). Optional query params: `Option<T>`.
- **Response Models:** Immutable records. Located in `Amadeus.Net.Endpoints.YourFeature.Response` or shared in `Amadeus.Net.Endpoints.Response`. Optionals: nullable types (e.g., `string?`).
- **Async:** All network ops are async (`InvokeAsync`). Error Handling: `Either<ErrorResponse, TResult>` (where `ErrorResponse` is from `Amadeus.Net.Endpoints.Response`).
- **Testing:** DI for `AmadeusContext`, call `context.Endpoint.Get(query).InvokeAsync(token)`, assert `Either`.

## Implementation Steps
1.  **Define Models (from OpenAPI Specification):**
    *   **Instruction:** The OpenAPI specification file for the targeted Amadeus API is the **source of truth**. Use it to meticulously define all required models for both the request and response. 
    *   **Response Models:**
        *   **Location & Namespace:** Place response model files in a dedicated `Response` subfolder within your new endpoint's directory (e.g., `src/Amadeus.Net/Endpoints/YourFeatureName/Response/`). The namespace for these models should reflect this structure (e.g., `Amadeus.Net.Endpoints.YourFeatureName.Response`).
        *   **Check for Reuse First:** Before creating new response models, **always** review the existing shared models in the `Amadeus.Net.Endpoints.Response` namespace (located in the `src/Amadeus.Net/Endpoints/Response/` folder). Common structures like `ApiError`, `Source`, `Warning`, `Meta`, `Dictionaries`, `Pagination` links, etc., may already exist and should be reused to maintain consistency and avoid duplication.
        *   **Implementation:** If new models are needed, implement them as sealed immutable C# records. Adhere to PascalCase naming for properties. Represent optional fields with nullable C# types (e.g., `string?`, `int?`). Apply the `[JsonPropertyName("api_field_name")]` attribute to properties when C# naming conventions diverge from the JSON field names in the API response (as defined in the OpenAPI spec).
    *   **Query Models/Parameters:** These are also derived from the OpenAPI specification. Place them in your endpoint's directory (e.g., `src/Amadeus.Net/Endpoints/YourFeatureName/YourQuery.cs`) with a namespace like `Amadeus.Net.Endpoints.YourFeatureName`. They are typically implemented as immutable C# records or classes that encapsulate all possible inputs to the API call (see Step 2 for detailed implementation).

2.  **Define Query Object:**
    *   **Instruction:** Create an immutable record or class for the endpoint's query parameters (e.g., `YourQuery.cs` in `Amadeus.Net.Endpoints.YourFeatureName`). This object encapsulates all possible inputs to the API call.
    *   **Interface:** It must implement the `IQuery` interface (from `Amadeus.Net.Endpoints.Query`).
    *   **Serialization:** Implement `ToParams() -> Seq<QueryParameter>`. This method is crucial for serializing *only the set* (i.e., `Some`) optional parameters and all required parameters into a sequence of `QueryParameter` objects (from `Amadeus.Net.Endpoints.Query`), suitable for constructing an HTTP query string.
    *   **Construction:**
        *   Provide a static `From(...)` factory method for instantiating the query object with all *required* parameters.
        *   Offer composable `With*` instance methods for each *optional* parameter. These methods should return a new instance of the query object with the specified optional parameter set.
    *   **Optional Parameters:** Represent optional query parameters within the query object using `LanguageExt.Option<T>`. This clearly distinguishes unset parameters from those intentionally set with a null-equivalent value if the API supported it (though typically, unset optionals are just omitted from the request).

3.  **Define Endpoint Logic and Register in `AmadeusContext`:**
    *   **Instruction:** Create a static factory method for your endpoint within a dedicated class for your endpoint's logic, and then register this endpoint in `AmadeusContext`.
    *   **Endpoint Logic Class & Factory Method(s):**
        *   Create an `internal sealed class YourFeatureName` (e.g., `MyFeature`) in your endpoint's directory (e.g., `src/Amadeus.Net/Endpoints/MyFeature/MyFeature.cs`). The namespace would be `Amadeus.Net.Endpoints.MyFeature`.
        *   For standard GET requests with static paths:
            *   Define a `public static Endpoint<Q, R> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData)` method.
            *   This method will typically use the `Endpoint.Create<Q, R>(httpClient, clientMetaData, "your/api/path")` factory (from `Amadeus.Net.Endpoints`).
            *   Define the API path as a `private const string Path = "/your/api/path";` within this class.
        *   For requests requiring dynamic path construction (e.g., ID in path) or other custom logic:
            *   Define a similar static factory method, e.g., `CreateEntityEndpoint(HttpClient httpClient, ClientMetaData clientMetaData)`.
            *   Inside this method, you might construct the `Endpoint<Q,R>` directly with a custom `GetFn` delegate:
                `return new Endpoint<Q, R>(query => httpClient.Get<Q, R>(clientMetaData, $"/your/api/path/{query.Id}", query));`
                (assuming `Q` has an `Id` property used in the path and `httpClient.Get` can handle it).
        *   The `ClientMetaData` (containing API version, client name) is passed from `AmadeusOptions` (available in `AmadeusContext`).
    *   **Register in `AmadeusContext`:**
        *   In `AmadeusContext.cs` (located at `src/Amadeus.Net/Context/AmadeusContext.cs`, namespace `Amadeus.Net.Context`), add a public, read-only property for your endpoint: `public Endpoint<Q, R> YourNewEndpointProperty { get; }`.
        *   Initialize this property by calling your endpoint logic class's static factory method, passing `httpClient` and `options.ClientMetaData` from `AmadeusContext`'s constructor parameters: `YourNewEndpointProperty = YourFeatureName.CreateEndpoint(httpClient, options.ClientMetaData);`.
    *   **Reference:** See the "Minimal Concrete Example" and existing endpoint registrations in `AmadeusContext.cs` for the precise pattern.

4.  **Implement xUnit Tests:**
    *   **Instruction:** Create comprehensive unit tests for the new endpoint to ensure its correctness and resilience.
    *   **Setup:** Use Dependency Injection (DI) to obtain an `AmadeusContext` instance. Configure necessary options (like `Amadeus:Host`) and secrets for the test environment.
    *   **Invocation:**
        *   Construct the query object using its static `From(...)` method and any relevant `With*` methods to simulate various request scenarios.
        *   Execute the endpoint call: `var result = await context.YourNewEndpointProperty.Get(query).InvokeAsync(cancellationToken);`.
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
// Query (e.g., in src/Amadeus.Net/Endpoints/MyFeature/MyQuery.cs)
// Namespace: Amadeus.Net.Endpoints.MyFeature
using Amadeus.Net.Endpoints.Query; // For IQuery, QueryParameter
using LanguageExt;

public sealed record MyQuery(
    string RequiredParameter,
    Option<string> OptionalParameter = default)
    : IQuery
{
    public static MyQuery From(string requiredParameter) => new(requiredParameter);
    public MyQuery WithOptionalParameter(string value) => this with { OptionalParameter = Prelude.Some(value) };
    public Seq<QueryParameter> ToParams() =>
        Prelude.Seq(
            Prelude.Some(QueryParameter.Create("requiredKey", RequiredParameter)),
            OptionalParameter.Map(val => QueryParameter.Create("optionalKey", val))
        ).Choose(option => option);
}

// Response (e.g., in src/Amadeus.Net/Endpoints/MyFeature/Response/MyResponse.cs)
// Namespace: Amadeus.Net.Endpoints.MyFeature.Response
using System.Text.Json.Serialization;

public sealed record MyResponse([property: JsonPropertyName("someField")] string? SomeField);

// Endpoint Logic (e.g., in src/Amadeus.Net/Endpoints/MyFeature/MyFeature.cs)
// Namespace: Amadeus.Net.Endpoints.MyFeature
using Amadeus.Net.Endpoints; // For Endpoint<Q,R> struct and static Endpoint.Create
using Amadeus.Net.Endpoints.MyFeature.Response; // For MyResponse
using Amadeus.Net.Options; // For ClientMetaData
using System.Net.Http; // For HttpClient
// Note: MyQuery is in the same namespace (Amadeus.Net.Endpoints.MyFeature)

internal static class MyFeature // Class name matches the feature/directory name
{
    private const string Path = "/v1/myendpoint"; // The specific API path for this endpoint

    // Factory method to create the endpoint instance
    public static Endpoint<MyQuery, MyResponse> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.Create<MyQuery, MyResponse>(httpClient, clientMetaData, Path);
}

// AmadeusContext.cs (Example: Illustrative snippet showing registration within your existing src/Amadeus.Net/Context/AmadeusContext.cs)
// Ensure you have the necessary using statements at the top of your AmadeusContext.cs file:
// using Amadeus.Net.Endpoints;
// using Amadeus.Net.Endpoints.MyFeature; // Or your specific feature namespace
// using Amadeus.Net.Endpoints.MyFeature.Response; // Or your specific feature response namespace
// using Amadeus.Net.Options;
// using System.Net.Http;

public sealed class AmadeusContext(HttpClient httpClient, AmadeusOptions options)
{
    // ... existing endpoint properties ...

    // Add your new endpoint property like this:
    public Endpoint<MyQuery, MyResponse> MyFeatureEndpoint { get; } = // Property name can be descriptive
        MyFeature.CreateEndpoint(httpClient, options.ClientMetaData);

    // ... any other existing endpoint properties ...
}

// --- In YourTestsProject/Clients/MyFeatureTests.cs (Note: Test folder might still be 'Clients') ---
// using Amadeus.Net.Context; // For AmadeusContext
// using Amadeus.Net.Endpoints.MyFeature; // For MyQuery
// using Amadeus.Net.Endpoints.MyFeature.Response; // For MyResponse (if needed for assertions on specific types)
// using Amadeus.Net.Endpoints.Response; // For ErrorResponse
// using LanguageExt;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using System.Reflection;
// using Xunit;
// using System.Threading.Tasks;
// using System.Threading;
// using System.Collections.Generic; // For Dictionary

// Assuming test class is in namespace Amadeus.Net.Tests.Clients or Amadeus.Net.Tests.Endpoints
public sealed class MyFeatureTests // Or a more descriptive name like MyFeatureEndpointTests
{
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "Amadeus:Host", "https://test.api.amadeus.com" }, // Or your test environment
            { "Amadeus:ClientMetaData:ClientVersion", "0.0.0" },
            { "Amadeus:ClientMetaData:ClientName", "Amadeus.Net.Tests" }, // Or your specific test client name
        }!)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true, false) // For API keys
        .Build();

    [Fact]
    public async Task MyFeatureEndpoint_WhenCalledWithValidQuery_ReturnsExpectedData()
    {
        var tkn = CancellationToken.None;

        var response = await Prelude.use(
            acquire: () => new ServiceCollection()
                .AddAmadeusContext(configuration) 
                .BuildServiceProvider(),
            release: provider => provider.Dispose())
            .Map(provider => provider.GetRequiredService<AmadeusContext>())
            .Bind(context => context.MyFeatureEndpoint.Get( // Assuming MyFeatureEndpoint is the property name in AmadeusContext
                MyQuery.From("someRequiredValue").WithOptionalParameter("optionalValue")))
            .InvokeAsync(tkn);

        _ = response.Match(
            Left: error => Assert.Fail($"expected success, but got error: {error}"),
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
For a full production example, refer to existing endpoint implementations like those under the `Amadeus.Net.Endpoints.AirlineCodeLookup` namespace.
