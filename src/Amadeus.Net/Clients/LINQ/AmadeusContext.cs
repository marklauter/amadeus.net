using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Models;

namespace Amadeus.Net.Clients.LINQ;

public sealed class AmadeusContext
{
    public Endpoint<Airlines, AirlineCodeFilter> Airlines { get; }

    public Endpoint<FlightDestinations, FlightInspirationFilter> FlightInspirations { get; }

    internal AmadeusContext(
        AirlineCodeLookupClient airlineCodeLookupClient,
        FlightInspirationClient inspirationClient)
    {
        Airlines = airlineCodeLookupClient.CreateEndpoint();
        FlightInspirations = inspirationClient.CreateEndpoint();
    }
}

