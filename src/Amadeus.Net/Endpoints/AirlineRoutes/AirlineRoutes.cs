// Amadeus.Net\Endpoints\AirlineRoutes\AirlineRoutes.cs
using Amadeus.Net.Endpoints.AirlineRoutes.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.Endpoints.AirlineRoutes;

internal sealed class AirlineRoutes
{
    private const string Path = "/v1/airline/destinations";

    public static Endpoint<AirlineRoutesQuery, AirlineRoutesResponse> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.MakeGet<AirlineRoutesQuery, AirlineRoutesResponse>(httpClient, clientMetaData, Path);
}