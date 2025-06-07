using Amadeus.Net.Endpoints.AirlineCodeLookup.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.Endpoints.AirlineCodeLookup;

internal sealed class AirlineCodeLookup
{
    private const string Path = "/v1/reference-data/airlines";

    public static Endpoint<AirlineCodeQuery, AirlineLookupResponse> CreateEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.MakeGet<AirlineCodeQuery, AirlineLookupResponse>(httpClient, clientMetaData, Path);
}
