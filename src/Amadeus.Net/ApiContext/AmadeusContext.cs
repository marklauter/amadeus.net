using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    //private Endpoint<AirlineLookupResponse, AirlineCodeFilter>? airlines;
    //public Endpoint<AirlineLookupResponse, AirlineCodeFilter> Airlines =>
    //    airlines ??= new AirlineCodeLookupClient(httpClient, options).CreateEndpoint();

    //private Endpoint<FlightInspirationResponse, FlightInspirationFilter>? flightInspirations;
    //public Endpoint<FlightInspirationResponse, FlightInspirationFilter> FlightInspirations =>
    //    flightInspirations ??= new FlightInspirationClient(httpClient, options).CreateEndpoint();

    private Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>>? airportCities;
    public Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>> AirportCities =>
        airportCities ??=
            new Endpoint<Either<AirportCitySearchFilter, LocationId>, Either<AirportCitySearchResponse, Location>>(
                new AirportCitySearchClient(httpClient, options).Filter);
}

