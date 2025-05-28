using Amadeus.Net.Clients;
using Amadeus.Net.Clients.Response;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

/// <summary>
/// Represents an operation that receives a query of type Q and returns an IO containing either an ErrorResponse or a response of type R.
/// The IO monad provided by <see cref="LanguageExt.IO{T}"/> encapsulates the deferred execution of the operation.
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
