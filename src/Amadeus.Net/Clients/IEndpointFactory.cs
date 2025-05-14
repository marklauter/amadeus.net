using Amadeus.Net.Clients.LINQ;

namespace Amadeus.Net.Clients;

internal interface IEndpointFactory<TResult, TFilter>
{
    Endpoint<TResult, TFilter> CreateEndpoint();
}
