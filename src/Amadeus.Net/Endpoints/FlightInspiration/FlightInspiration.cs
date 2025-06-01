using Amadeus.Net.Endpoints.FlightInspiration.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.Endpoints.FlightInspiration;

internal sealed class FlightInspiration
{
    private const string Path = "/v1/shopping/flight-destinations";

    public static Endpoint<FlightInspirationQuery, FlightInspirationResponse> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.Create<FlightInspirationQuery, FlightInspirationResponse>(httpClient, clientMetaData, Path);
}
