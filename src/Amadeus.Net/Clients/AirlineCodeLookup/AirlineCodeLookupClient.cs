using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

internal sealed class AirlineCodeLookupClient
{
    private const string Path = "/v1/reference-data/airlines";

    public static Endpoint<AirlineCodeQuery, AirlineLookupResponse> CreateEndpoint(
        HttpClient httpClient,
        AmadeusOptions options) =>
            new(query =>
                httpClient.Get<AirlineCodeQuery, AirlineLookupResponse>(options, Path, query));
}
