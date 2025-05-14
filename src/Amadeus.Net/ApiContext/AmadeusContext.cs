using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Models;
using Amadeus.Net.Options;

namespace Amadeus.Net.ApiContext;

public sealed class AmadeusContext(
    HttpClient httpClient,
    AmadeusOptions options)
{
    public Endpoint<Airlines, AirlineCodeFilter> Airlines { get; } = new AirlineCodeLookupClient(httpClient, options).CreateEndpoint();
    public Endpoint<FlightDestinations, FlightInspirationFilter> FlightInspirations { get; } = new FlightInspirationClient(httpClient, options).CreateEndpoint();
}

