using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private readonly Lazy<Endpoint<AirlineCodeQuery, AirlineLookupResponse>> airlines =
        new(() =>
            new Endpoint<AirlineCodeQuery, AirlineLookupResponse>(
                Get: filter => httpClient.Filter<AirlineCodeQuery, AirlineLookupResponse>(
                    options,
                    AirlineCodeLookupClient.Path,
                    filter)));
    public Endpoint<AirlineCodeQuery, AirlineLookupResponse> Airlines => airlines.Value;

    private readonly Lazy<Endpoint<FlightInspirationQuery, FlightInspirationResponse>> flightInspirations =
        new(() =>
            new Endpoint<FlightInspirationQuery, FlightInspirationResponse>(
                Get: filter => httpClient.Filter<FlightInspirationQuery, FlightInspirationResponse>(
                    options,
                    FlightInspirationClient.Path,
                    filter)));
    public Endpoint<FlightInspirationQuery, FlightInspirationResponse> FlightInspirations => flightInspirations.Value;

    private readonly Lazy<Endpoint<AirportCityQuery, AirportCitySearchResponse>> airportCities =
        new(() =>
            new Endpoint<AirportCityQuery, AirportCitySearchResponse>(
                    Get: filter => httpClient.Filter<AirportCityQuery, AirportCitySearchResponse>(
                        options,
                        AirportCitySearchClient.Path,
                        filter)));
    public Endpoint<AirportCityQuery, AirportCitySearchResponse> AirportCities => airportCities.Value;

    private readonly Lazy<Endpoint<LocationId, Location>> airportCity =
        new(() =>
            new Endpoint<LocationId, Location>(
                    Get: locationId => httpClient.Filter<LocationId, Location>(
                        options,
                        $"{AirportCitySearchClient.Path}/{locationId}",
                        locationId)));
    public Endpoint<LocationId, Location> AirportCity => airportCity.Value;
}

