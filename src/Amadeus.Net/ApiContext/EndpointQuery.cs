using Amadeus.Net.Clients.Models;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class EndpointQuery<TResult, TFilter>
{
    private readonly Endpoint<TResult, TFilter> endpoint;
    private readonly Func<Option<TFilter>> filterFactory;

    internal EndpointQuery(Endpoint<TResult, TFilter> endpoint, Func<Option<TFilter>> filterFactory)
    {
        this.endpoint = endpoint;
        this.filterFactory = filterFactory;
    }

    public Task<Either<ErrorResponse, TResult>> ExecuteReaderAsync(CancellationToken cancellationToken) =>
        endpoint.ReadAsync(filterFactory(), cancellationToken);
}
