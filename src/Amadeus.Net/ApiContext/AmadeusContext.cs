using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Clients.AirportCitySearch.Models;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Models;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private Endpoint<Airlines, AirlineCodeFilter>? airlines;
    public Endpoint<Airlines, AirlineCodeFilter> Airlines =>
        airlines ??= new AirlineCodeLookupClient(httpClient, options).CreateEndpoint();

    private Endpoint<FlightDestinations, FlightInspirationFilter>? flightInspirations;
    public Endpoint<FlightDestinations, FlightInspirationFilter> FlightInspirations =>
        flightInspirations ??= new FlightInspirationClient(httpClient, options).CreateEndpoint();

    private Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>>? airportCities;
    public Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>> AirportCities =>
        airportCities ??= new AirportCitySearchClient(httpClient, options).CreateEndpoint();
}

