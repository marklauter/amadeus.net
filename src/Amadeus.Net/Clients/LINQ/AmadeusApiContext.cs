using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Clients.FlightInspiration;
using Amadeus.Net.Clients.FlightInspiration.Models;

namespace Amadeus.Net.Clients.LINQ;

public sealed class AmadeusApiContext
{
    public Endpoint<Airlines, AirlineCodeFilter> Airlines { get; }

    public Endpoint<FlightDestinations, FlightInspirationFilter> FlightInspirations { get; }

    internal AmadeusApiContext(
        AirlineCodeLookupClient airlineCodeLookupClient,
        FlightInspirationClient inspirationClient)
    {
        Airlines =
            new Endpoint<Airlines, AirlineCodeFilter>(
                (filter, ct) =>
                    filter.Match(
                        Some: f => airlineCodeLookupClient.TryGetAirlinesByCodesAsync(f.Codes.ToOption(), ct),
                        None: airlineCodeLookupClient.TryGetAllAirlinesAsync(ct)
                    )
            );
        FlightInspirations =
            new Endpoint<FlightDestinations, FlightInspirationFilter>(inspirationClient.TryGetFlightInspirationsAsync);
    }
}

