using Amadeus.Net.Endpoints;
using Amadeus.Net.Endpoints.AirlineCodeLookup;
using Amadeus.Net.Endpoints.AirlineCodeLookup.Response;
using Amadeus.Net.Endpoints.AirlineRoutes;
using Amadeus.Net.Endpoints.AirlineRoutes.Response;
using Amadeus.Net.Endpoints.AirportCitySearch;
using Amadeus.Net.Endpoints.AirportCitySearch.Response;
using Amadeus.Net.Endpoints.FlightInspiration;
using Amadeus.Net.Endpoints.FlightInspiration.Response;
using Amadeus.Net.Options;

namespace Amadeus.Net.Context;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    public Endpoint<AirlineCodeQuery, AirlineLookupResponse> Airlines { get; } =
        AirlineCodeLookup.CreateEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<FlightInspirationQuery, FlightInspirationResponse> FlightInspirations { get; } =
        FlightInspiration.CreateEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<AirportCityQuery, AirportCitySearchResponse> AirportCities { get; } =
        AirportCitySearch.CreateSearchEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<LocationId, Endpoints.AirportCitySearch.Response.Location> AirportCity { get; } =
        AirportCitySearch.CreateLocationEndpoint(httpClient, options.ClientMetaData);

    public Endpoint<AirlineRoutesQuery, AirlineRoutesResponse> AirlineRoutes { get; } =
        Endpoints.AirlineRoutes.AirlineRoutes.CreateEndpoint(httpClient, options.ClientMetaData);
}

