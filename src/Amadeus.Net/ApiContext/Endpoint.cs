using Amadeus.Net.Clients.Models;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class Endpoint<TResult, TFilter>(Func<TFilter, CancellationToken, Task<Either<ErrorResponse, TResult>>> readFunc)
{
    private readonly Func<TFilter, CancellationToken, Task<Either<ErrorResponse, TResult>>> readFunc =
        readFunc ?? throw new ArgumentNullException(nameof(readFunc));

    public EndpointQuery<TResult, TFilter> FilterBy(Func<TFilter> filterFactory) =>
        new(this, filterFactory);

    internal Task<Either<ErrorResponse, TResult>> ReadAsync(TFilter filter, CancellationToken cancellationToken) =>
        readFunc(filter, cancellationToken);
}
