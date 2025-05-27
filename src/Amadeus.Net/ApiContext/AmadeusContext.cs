using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private Endpoint<AirlineLookupResponse, AirlineCodeFilter>? airlines;
    public Endpoint<AirlineLookupResponse, AirlineCodeFilter> Airlines =>
        airlines ??= new AirlineCodeLookupClient(httpClient, options).CreateEndpoint();

    private Endpoint<FlightInspirationResponse, FlightInspirationFilter>? flightInspirations;
    public Endpoint<FlightInspirationResponse, FlightInspirationFilter> FlightInspirations =>
        flightInspirations ??= new FlightInspirationClient(httpClient, options).CreateEndpoint();

    //private Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>>? airportCities;
    //public Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>> AirportCities =>
    //    airportCities ??= new AirportCitySearchClient(httpClient, options).CreateEndpoint();
}

