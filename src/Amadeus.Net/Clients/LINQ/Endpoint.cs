using Amadeus.Net.Clients.Models;
using LanguageExt;

namespace Amadeus.Net.Clients.LINQ;

public sealed class Endpoint<TResult, TFilter>(
    Func<Option<TFilter>, CancellationToken, Task<Either<ErrorResponse, TResult>>> fetchFunc)
{
    private readonly Func<Option<TFilter>, CancellationToken, Task<Either<ErrorResponse, TResult>>> fetchFunc =
        fetchFunc ?? throw new ArgumentNullException(nameof(fetchFunc));

    public Task<Either<ErrorResponse, TResult>> ReadAsync(Option<TFilter> filter, CancellationToken cancellationToken)
        => fetchFunc(filter, cancellationToken);
}
