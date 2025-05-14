using Amadeus.Net.Clients.Models;
using System.Globalization;

namespace Amadeus.Net.Clients.FlightInspiration;

public sealed record FlightInspirationFilter(
    IataCode Origin,
    DepartureDateSelection? DepartureDate = null,
    bool? OneWay = null,
    int? TripDurationDays = null,
    bool? NonStop = null,
    int? MaxPrice = null
)
{
    public IEnumerable<KeyValuePair<string, string>> AsQueryParams()
    {
        yield return KeyValuePair.Create("origin", Origin.ToString());

        if (DepartureDate is not null)
            yield return KeyValuePair.Create("departureDate", DepartureDate.ToString());

        if (OneWay.HasValue)
            yield return KeyValuePair.Create("oneWay", OneWay.Value.ToString().ToLowerInvariant());

        if (TripDurationDays.HasValue)
            yield return KeyValuePair.Create("duration", TripDurationDays.Value.ToString(CultureInfo.InvariantCulture));

        if (NonStop.HasValue)
            yield return KeyValuePair.Create("nonStop", NonStop.Value.ToString().ToLowerInvariant());

        if (MaxPrice.HasValue)
            yield return KeyValuePair.Create("maxPrice", MaxPrice.Value.ToString(CultureInfo.InvariantCulture));
    }
}

