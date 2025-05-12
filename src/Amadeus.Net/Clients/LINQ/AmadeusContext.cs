using Amadeus.Net.Clients.AirlineCodeLookup;
using Amadeus.Net.Clients.AirlineCodeLookup.Models;

namespace Amadeus.Net.Clients.LINQ;

public class AmadeusContext
{
    public Endpoint<Airlines, AirlineCodeFilter> Airlines { get; }

    internal AmadeusContext(AirlineCodeLookupClient airlineClient) =>
        Airlines = new Endpoint<Airlines, AirlineCodeFilter>(
            (filter, ct) =>
                filter.Match(
                    Some: f => airlineClient.TryGetAirlinesByCodesAsync(f.Codes.ToOption(), ct),
                    None: airlineClient.TryGetAllAirlinesAsync(ct)
                )
        );
}
