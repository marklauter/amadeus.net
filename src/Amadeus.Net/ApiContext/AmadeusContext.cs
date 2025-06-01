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
    private readonly Lazy<Endpoint<AirlineCodeQuery, AirlineLookupResponse>> airlines =
        new(() => AirlineCodeLookupClient.CreateEndpoint(httpClient, options));
    public Endpoint<AirlineCodeQuery, AirlineLookupResponse> Airlines => airlines.Value;

    private readonly Lazy<Endpoint<FlightInspirationQuery, FlightInspirationResponse>> flightInspirations =
        new(() => FlightInspirationClient.CreateEndpoint(httpClient, options));
    public Endpoint<FlightInspirationQuery, FlightInspirationResponse> FlightInspirations => flightInspirations.Value;

    private readonly Lazy<Endpoint<AirportCityQuery, AirportCitySearchResponse>> airportCities =
        new(() => AirportCitySearchClient.CreateSearchEndpoint(httpClient, options));
    public Endpoint<AirportCityQuery, AirportCitySearchResponse> AirportCities => airportCities.Value;

    private readonly Lazy<Endpoint<LocationId, Location>> airportCity =
        new(() => AirportCitySearchClient.CreateLocationEndpoint(httpClient, options));
    public Endpoint<LocationId, Location> AirportCity => airportCity.Value;
}

