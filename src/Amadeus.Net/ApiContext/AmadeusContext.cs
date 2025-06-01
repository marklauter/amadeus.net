using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    public Endpoint<AirlineCodeQuery, AirlineLookupResponse> Airlines { get; } =
        AirlineCodeLookupClient.CreateEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<FlightInspirationQuery, FlightInspirationResponse> FlightInspirations { get; } =
        FlightInspirationClient.CreateEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<AirportCityQuery, AirportCitySearchResponse> AirportCities { get; } =
        AirportCitySearchClient.CreateSearchEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<LocationId, Location> AirportCity { get; } =
        AirportCitySearchClient.CreateLocationEndpoint(httpClient, options.ClientMetaData);
}

