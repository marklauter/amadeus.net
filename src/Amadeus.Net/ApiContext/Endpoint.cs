using Amadeus.Net.Clients.Models;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class Endpoint<TResult, TFilter>(Func<Option<TFilter>, CancellationToken, Task<Either<ErrorResponse, TResult>>> readFunc)
{
    private readonly Func<Option<TFilter>, CancellationToken, Task<Either<ErrorResponse, TResult>>> readFunc =
        readFunc ?? throw new ArgumentNullException(nameof(readFunc));

    public EndpointQuery<TResult, TFilter> FilterBy(Func<Option<TFilter>> filterFactory) =>
        new(this, filterFactory);

    public Task<Either<ErrorResponse, TResult>> ReadAsync(Option<TFilter> filter, CancellationToken cancellationToken) =>
        readFunc(filter, cancellationToken);
}
