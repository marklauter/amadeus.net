using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private Endpoint<AirlineCodeFilter, AirlineLookupResponse>? airlines;
    public Endpoint<AirlineCodeFilter, AirlineLookupResponse> Airlines =>
        airlines ??=
            new Endpoint<AirlineCodeFilter, AirlineLookupResponse>(
                new AirlineCodeLookupClient(httpClient, options).Filter);

    private Endpoint<FlightInspirationFilter, FlightInspirationResponse>? flightInspirations;
    public Endpoint<FlightInspirationFilter, FlightInspirationResponse> FlightInspirations =>
        flightInspirations ??=
            new Endpoint<FlightInspirationFilter, FlightInspirationResponse>(
                new FlightInspirationClient(httpClient, options).Filter);

    private Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>>? airportCities;
    public Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>> AirportCities =>
        airportCities ??=
            new Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>>(
                new AirportCitySearchClient(httpClient, options).Filter);
}

