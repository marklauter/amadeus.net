using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.Clients.FlightInspiration;

internal sealed class FlightInspirationClient
{
    private const string Path = "/v1/shopping/flight-destinations";

    public static Endpoint<FlightInspirationQuery, FlightInspirationResponse> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.Create<FlightInspirationQuery, FlightInspirationResponse>(httpClient, clientMetaData, Path);
}
