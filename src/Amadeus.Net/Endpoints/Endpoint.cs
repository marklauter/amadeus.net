using Amadeus.Net.Endpoints.Query;
using Amadeus.Net.Endpoints.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.Endpoints;

/// <summary>
/// Represents an operation that receives a query of type Q and returns an IO containing either an ErrorResponse or a response of type R.
/// The IO monad provided by <see cref="IO{T}"/> encapsulates the deferred execution of the operation.
/// </summary>
/// <typeparam name="Q">The type of query. Must implement IQuery.</typeparam>
/// <typeparam name="R">The type of response.</typeparam>
public delegate IO<Either<ErrorResponse, R>> GetOperation<in Q, R>(Q query) where Q : IQuery;

/// <summary>
/// Represents an endpoint that encapsulates a GET operation for specific query and response types.
/// </summary>
/// <typeparam name="Q">The type of query. Must implement IQuery.</typeparam>
/// <typeparam name="R">The type of response.</typeparam>
public readonly record struct Endpoint<Q, R>(GetOperation<Q, R> Get) where Q : IQuery;

/// <summary>
/// Provides factory methods for creating endpoint instances for specific query and response types. 
/// These endpoints execute operations and wrap their execution in an IO monad to support deferred execution.
/// </summary>
public static class Endpoint
{
    /// <summary>
    /// Creates a new endpoint instance that encapsulates a GET operation for the specified query and response types.
    /// The underlying HTTP client is used to perform the GET request with the provided client metadata and path.
    /// </summary>
    /// <typeparam name="Q">The type of query used in the GET operation. It must implement <see cref="IQuery"/>.</typeparam>
    /// <typeparam name="R">The type of response returned by the GET operation.</typeparam>
    /// <param name="httpClient">The HTTP client instance used to send the GET request.</param>
    /// <param name="clientMetaData">Metadata required by the client for authentication and request configuration.</param>
    /// <param name="path">The endpoint path used to compose the GET request.</param>
    /// <returns>An <see cref="Endpoint{Q, R}"/> instance encapsulating the GET operation.</returns>
    public static Endpoint<Q, R> Create<Q, R>(HttpClient httpClient, ClientMetaData clientMetaData, string path) where Q : IQuery =>
        new(Get: query => httpClient.Get<Q, R>(clientMetaData, path, query));
}
