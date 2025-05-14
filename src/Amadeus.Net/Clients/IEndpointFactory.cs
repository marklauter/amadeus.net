using Amadeus.Net.ApiContext;

namespace Amadeus.Net.Clients;

internal interface IEndpointFactory<TResult, TFilter>
{
    Endpoint<TResult, TFilter> CreateEndpoint();
}
