using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;

namespace Amadeus.Net.Clients.AirportCitySearch;

internal sealed class AirportCitySearchClient
{
    private const string Path = "/v1/reference-data/locations";

    public static Endpoint<AirportCityQuery, AirportCitySearchResponse> CreateSearchEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        Endpoint.Create<AirportCityQuery, AirportCitySearchResponse>(httpClient, clientMetaData, Path);

    public static Endpoint<LocationId, Location> CreateLocationEndpoint(HttpClient httpClient, ClientMetaData clientMetaData) =>
        new(locationId => httpClient.Get<LocationId, Location>(clientMetaData, $"{Path}/{locationId}", locationId));
}
