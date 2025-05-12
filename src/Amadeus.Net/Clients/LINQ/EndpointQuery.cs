using Amadeus.Net.Clients.Models;
using LanguageExt;

namespace Amadeus.Net.Clients.LINQ;

public sealed class EndpointQuery<TResult, TFilter>
{
    private readonly Endpoint<TResult, TFilter> endpoint;
    private readonly Func<Option<TFilter>> filterFactory;

    internal EndpointQuery(Endpoint<TResult, TFilter> endpoint, Func<Option<TFilter>> filterFactory)
    {
        this.endpoint = endpoint;
        this.filterFactory = filterFactory;
    }

    public async Task<Either<ErrorResponse, IReadOnlyList<TElement>>> ToListAsync<TElement>(
        CancellationToken cancellationToken,
        Func<TResult, IReadOnlyList<TElement>> selector)
    {
        var filter = filterFactory();
        var result = await endpoint.ReadAsync(filter, cancellationToken);
        return result.Map(selector);
    }

    // Additional LINQ-like methods (FirstOrDefaultAsync, etc.) can be added here
}
