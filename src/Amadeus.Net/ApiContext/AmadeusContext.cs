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
    private readonly Lazy<Endpoint<AirlineCodeFilter, AirlineLookupResponse>> airlines =
        new(() =>
            new Endpoint<AirlineCodeFilter, AirlineLookupResponse>(
                operation: filter => httpClient.Filter<AirlineCodeFilter, AirlineLookupResponse>(
                    options,
                    AirlineCodeLookupClient.Path,
                    filter)));
    public Endpoint<AirlineCodeFilter, AirlineLookupResponse> Airlines => airlines.Value;

    private readonly Lazy<Endpoint<FlightInspirationFilter, FlightInspirationResponse>> flightInspirations =
        new(() =>
            new Endpoint<FlightInspirationFilter, FlightInspirationResponse>(
                operation: filter => httpClient.Filter<FlightInspirationFilter, FlightInspirationResponse>(
                    options,
                    FlightInspirationClient.Path,
                    filter)));
    public Endpoint<FlightInspirationFilter, FlightInspirationResponse> FlightInspirations => flightInspirations.Value;

    // todo: make lazy
    private Endpoint<AirportCitySearchFilter, AirportCitySearchResponse>? airportCities;
    public Endpoint<AirportCitySearchFilter, AirportCitySearchResponse> AirportCities =>
        airportCities ??=
            new Endpoint<AirportCitySearchFilter, AirportCitySearchResponse>(
                operation: filter => httpClient.Filter<AirportCitySearchFilter, AirportCitySearchResponse>(
                    options,
                    AirportCitySearchClient.Path,
                    filter));

    private Endpoint<LocationId, Location>? airportCity;
    public Endpoint<LocationId, Location> AirportCity =>
        airportCity ??=
            new Endpoint<LocationId, Location>(
                operation: locationId => httpClient.Filter<LocationId, Location>(
                    options,
                    $"{AirportCitySearchClient.Path}/{locationId}",
                    locationId));
}

