using Amadeus.Net.Clients.Models;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using System.Globalization;

namespace Amadeus.Net.Clients.FlightInspiration;

public sealed record FlightInspirationFilter(
    IataCode Origin,
    Option<TravelDates> TravelDates,
    Option<bool> OneWay,
    Option<int> TripDurationDays,
    Option<bool> NonStop,
    Option<int> MaxPrice)
{
    public static FlightInspirationFilter From(IataCode origin) => new(
        origin,
        Option<TravelDates>.None,
        Option<bool>.None,
        Option<int>.None,
        Option<bool>.None,
        Option<int>.None);

    public FlightInspirationFilter WithOrigin(IataCode origin) => this with { Origin = origin };
    public FlightInspirationFilter WithDepartureDate(TravelDates travelDates) => this with { TravelDates = travelDates };
    public FlightInspirationFilter WithOneWay(bool oneWay) => this with { OneWay = oneWay };
    public FlightInspirationFilter WithTripDuration(int days) => this with { TripDurationDays = days };
    public FlightInspirationFilter WithNonStop(bool nonStop) => this with { NonStop = nonStop };
    public FlightInspirationFilter WithMaxPrice(int maxPrice) => this with { MaxPrice = maxPrice };

    public IEnumerable<KeyValuePair<string, string>> AsQueryParams()
    {
        yield return KeyValuePair.Create("origin", Origin.ToString());

        if (TravelDates.IsSome)
            yield return KeyValuePair.Create("departureDate", TravelDates.ValueUnsafe().ToString());

        if (OneWay.IsSome)
            yield return KeyValuePair.Create("oneWay", OneWay.ValueUnsafe().ToString().ToLowerInvariant());

        if (TripDurationDays.IsSome)
            yield return KeyValuePair.Create("duration", TripDurationDays.ValueUnsafe().ToString(CultureInfo.InvariantCulture));

        if (NonStop.IsSome)
            yield return KeyValuePair.Create("nonStop", NonStop.ValueUnsafe().ToString().ToLowerInvariant());

        if (MaxPrice.IsSome)
            yield return KeyValuePair.Create("maxPrice", MaxPrice.ValueUnsafe().ToString(CultureInfo.InvariantCulture));
    }
}

