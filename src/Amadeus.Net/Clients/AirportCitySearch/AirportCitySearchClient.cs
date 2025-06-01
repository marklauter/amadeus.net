using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;

namespace Amadeus.Net.Clients.AirportCitySearch;

internal sealed class AirportCitySearchClient
{
    private const string Path = "/v1/reference-data/locations";

    public static Endpoint<AirportCityQuery, AirportCitySearchResponse> CreateSearchEndpoint(
        HttpClient httpClient,
        AmadeusOptions options) =>
            new(query =>
                httpClient.Get<AirportCityQuery, AirportCitySearchResponse>(options, Path, query));

    public static Endpoint<LocationId, Location> CreateLocationEndpoint(
        HttpClient httpClient,
        AmadeusOptions options) =>
            new(locationId =>
                httpClient.Get<LocationId, Location>(options, $"{Path}/{locationId}", locationId));
}
