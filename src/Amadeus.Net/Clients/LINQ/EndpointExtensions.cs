using LanguageExt;

namespace Amadeus.Net.Clients.LINQ;

public static class EndpointExtensions
{
    public static EndpointQuery<TResult, TFilter> Where<TResult, TFilter>(
        this Endpoint<TResult, TFilter> endpoint,
        Func<Option<TFilter>> filterFactory)
        => new(endpoint, filterFactory);
}
