# Amadeus.Net Client Implementation Instructions

This guide describes the patterns and steps for implementing new Amadeus for Devs API endpoints in the Amadeus.Net client, based on the established design in `ApiContext`, `AirlineCodeLookup`, and `FlightInspiration`.

---

## 1. Patterns in Use

### a. ApiContext Namespace

- **AmadeusContext**: Main entry point, constructed with `HttpClient` and `AmadeusOptions`.
    - Exposes properties for each API area as `Endpoint<TResult, TFilter>`.
    - Each property is initialized via a corresponding client’s `CreateEndpoint()` method.
- **Endpoint<TResult, TFilter>**:
    - Encapsulates a function to read results (`readFunc`).
    - Exposes `FilterBy(Func<Option<TFilter>>)`, returning an `EndpointQuery`.
    - Exposes `ReadAsync` to execute the query.
- **EndpointQuery<TResult, TFilter>**:
    - Holds a reference to the endpoint and a filter factory.
    - Exposes `ExecuteReaderAsync(CancellationToken)` to run the query.

### b. AirlineCodeLookup Namespace

- **AirlineCodeLookupClient**:
    - Implements `IEndpointFactory<Airlines, AirlineCodeFilter>`.
    - `CreateEndpoint` returns an `Endpoint` using a lambda that dispatches to either `TryGetAirlinesByCodesAsync` or `TryGetAllAirlinesAsync` based on the filter.
    - Uses a private `BuildRequest` helper for HTTP requests.
    - Uses `SendAsync` to send the HTTP request and parse the response.
- **AirlineCodeFilter**:
    - Immutable, exposes a collection of codes.
    - Static methods for `Some` (with codes) and `None` (no filter).

### c. FlightInspiration Namespace

- **FlightInspirationClient**:
    - Implements `IEndpointFactory<FlightDestinations, FlightInspirationFilter>`.
    - `CreateEndpoint` returns an `Endpoint` using `TryGetFlightInspirationsAsync`.
    - Builds HTTP requests using a private `BuildRequest` helper.
    - Uses `AsQueryParams()` from the filter to construct query parameters.
- **FlightInspirationFilter**:
    - Immutable record with origin, travel dates, one-way, duration, non-stop, max price.
    - `From` static method for required fields.
    - `With*` methods for optional fields.
    - `AsQueryParams()` yields query parameters for set properties only.

### d. General Patterns

- **Immutability**: Filters are immutable (record or class with readonly properties).
- **Option<T>**: Used for optionality in filters, but **API models should use regular dotnet nullable types for optional fields** (e.g., `string?`, `int?`, etc.) to ensure correct deserialization.
- **Endpoint Factory**: Each client implements `IEndpointFactory<TResult, TFilter>`, exposing a `CreateEndpoint()` method.
- **Request Building**: Common pattern for constructing requests with user agent, accept headers, and query parameters.
- **Async/Await**: All network operations are async.
- **Error Handling**: Uses `Either<ErrorResponse, TResult>` for result/error.
- **Unit Test Usage**: The test uses dependency injection to get `AmadeusContext`, then chains `.FlightInspirations.FilterBy(...).ExecuteReaderAsync(...)` for a query.

---

## 2. Implementation Guide for New OpenAPI Specs

### Step 1: Define Models

- Parse the OpenAPI/Swagger spec to identify request and response models.
- Implement sealed, immutable data models (prefer record types for responses, classes for filters).
- Use PascalCase for public types and properties.
- Use **dotnet nullable types** (e.g., `string?`, `int?`, etc.) for optional fields in API models. Do **not** use `Option<T>` in API models, as it will not deserialize.

### Step 2: Implement Filter Class/Record

- Represent all possible query parameters as immutable properties.
- Use static methods for required fields (e.g., `From(...)`).
- Provide composable `With*` methods for optional fields, returning a new instance.
- Implement `AsQueryParams()`, as required, to yield only set query parameters.
- Use private constructors.
- Use Option<T> for optional parameters.

### Step 3: Implement Client

- Implement `IEndpointFactory<TResult, TFilter>`.
- Provide a `CreateEndpoint()` method returning `Endpoint<TResult, TFilter>`.
- In the endpoint lambda, dispatch to appropriate internal methods based on filter presence/content.
- Internal methods should:
    - Build the request using a helper (set method, path, headers, and query).
    - Send the request and parse the response using `.TryParseAsync<TResult>()`.
    - Return `Either<ErrorResponse, TResult>`.

### Step 4: Register in AmadeusContext

- Add a property to `AmadeusContext` for the new endpoint:
    - `public Endpoint<TResult, TFilter> NewEndpoint { get; } = new NewClient(httpClient, options).CreateEndpoint();`

### Step 5: Unit Test

- Use dependency injection to get `AmadeusContext`.
- Chain `context.NewEndpoint.FilterBy(() => FilterType.From(...)).ExecuteReaderAsync(...)`.
- Assert on the result, e.g., that data is present.

---

## 3. Example Skeleton

```csharp
// Filter
public sealed record NewApiFilter(string RequiredParam, Option<int> OptionalParam) {
    public static NewApiFilter From(string required) => new(required, Option<int>.None);
    public NewApiFilter WithOptional(int value) => this with { OptionalParam = value };
    public IEnumerable<KeyValuePair<string, string>> AsQueryParams() {
        yield return KeyValuePair.Create("required", RequiredParam);
        if (OptionalParam.IsSome)
            yield return KeyValuePair.Create("optional", OptionalParam.ValueUnsafe().ToString());
    }
}

// API Model (Response/Request)
public sealed record NewApiResponse(
    string? OptionalField, 
    int? OptionalNumber
    // ... other fields
    );


// Client
internal sealed class NewApiClient(HttpClient httpClient, AmadeusOptions options)
    : IEndpointFactory<NewApiResponse, NewApiFilter> {
    private const string Path = "/v1/...";
    public Endpoint<NewApiResponse, NewApiFilter> CreateEndpoint() =>
        new((filter, ct) => TryGetSomethingAsync(filter, ct));
    internal async Task<Either<ErrorResponse, NewApiResponse>> TryGetSomethingAsync(
        Option<NewApiFilter> filter, CancellationToken ct) {
        var queryParams = filter.Match(
            Some: f => f.AsQueryParams().ToArray(),
            None: () => []
        );
        using var request = BuildRequest(HttpMethod.Get, Path, queryParams);
        using var response = await httpClient.SendAsync(request, ct);
        return await response.TryParseAsync<NewApiResponse>(ct);
    }
    // BuildRequest helper as in other clients
}

// AmadeusContext
public Endpoint<NewApiResponse, NewApiFilter> NewApi => new NewApiClient(httpClient, options).CreateEndpoint();
```

---

## 4. Summary Table

| Step                | Pattern/Requirement                                                                 |
|---------------------|-------------------------------------------------------------------------------------|
| Models              | Immutable, PascalCase, **dotnet nullable types for optional fields**                |
| Filter              | Static `From` for required, `With*` for optional, `AsQueryParams()` yields only set |
| Client              | Implements `IEndpointFactory`, `CreateEndpoint()`, builds/sends requests            |
| Endpoint/Context    | Add property to `AmadeusContext`, initialized via client                            |
| Error Handling      | Use `Either<ErrorResponse, TResult>` throughout                                     |
| Async               | All network methods are async                                                       |
| Test                | Use DI, chain `.FilterBy(...).ExecuteReaderAsync(...)`                              |

---

If you need a concrete example for a specific OpenAPI spec, let us know which one and we’ll scaffold it for you!
