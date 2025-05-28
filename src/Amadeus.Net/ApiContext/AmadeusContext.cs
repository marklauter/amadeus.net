using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private readonly Lazy<Endpoint<AirlineCodeQuery, AirlineLookupResponse>> airlines =
        new(() =>
            new Endpoint<AirlineCodeQuery, AirlineLookupResponse>(
                Get: query => httpClient.Get<AirlineCodeQuery, AirlineLookupResponse>(
                    options,
                    AirlineCodeLookupClient.Path,
                    query)));
    public Endpoint<AirlineCodeQuery, AirlineLookupResponse> Airlines => airlines.Value;

    private readonly Lazy<Endpoint<FlightInspirationQuery, FlightInspirationResponse>> flightInspirations =
        new(() =>
            new Endpoint<FlightInspirationQuery, FlightInspirationResponse>(
                Get: query => httpClient.Get<FlightInspirationQuery, FlightInspirationResponse>(
                    options,
                    FlightInspirationClient.Path,
                    query)));
    public Endpoint<FlightInspirationQuery, FlightInspirationResponse> FlightInspirations => flightInspirations.Value;

    private readonly Lazy<Endpoint<AirportCityQuery, AirportCitySearchResponse>> airportCities =
        new(() =>
            new Endpoint<AirportCityQuery, AirportCitySearchResponse>(
                    Get: query => httpClient.Get<AirportCityQuery, AirportCitySearchResponse>(
                        options,
                        AirportCitySearchClient.Path,
                        query)));
    public Endpoint<AirportCityQuery, AirportCitySearchResponse> AirportCities => airportCities.Value;

    private readonly Lazy<Endpoint<LocationId, Location>> airportCity =
        new(() =>
            new Endpoint<LocationId, Location>(
                    Get: query => httpClient.Get<LocationId, Location>(
                        options,
                        $"{AirportCitySearchClient.Path}/{query}",
                        query)));
    public Endpoint<LocationId, Location> AirportCity => airportCity.Value;
}

