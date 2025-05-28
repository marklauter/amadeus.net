using LanguageExt;
using System.Globalization;

namespace Amadeus.Net.Clients.FlightInspiration;

public sealed record FlightInspirationQuery(
    IataCode Origin,
    Option<TravelDates> TravelDates,
    Option<bool> OneWay,
    Option<int> TripDurationDays,
    Option<bool> NonStop,
    Option<int> MaxPrice)
    : IQuery
{
    public static FlightInspirationQuery From(IataCode origin) => new(
        origin,
        Option<TravelDates>.None,
        Option<bool>.None,
        Option<int>.None,
        Option<bool>.None,
        Option<int>.None);

    public FlightInspirationQuery WithTravelDates(TravelDates travelDates) => this with { TravelDates = travelDates };
    public FlightInspirationQuery WithOneWay(bool oneWay) => this with { OneWay = oneWay };
    public FlightInspirationQuery WithTripDuration(int days) => this with { TripDurationDays = days };
    public FlightInspirationQuery WithNonStop(bool nonStop) => this with { NonStop = nonStop };
    public FlightInspirationQuery WithMaxPrice(int maxPrice) => this with { MaxPrice = maxPrice };

    public Seq<KeyValuePair<string, string>> ToParams() =>
        Prelude.Seq(
            Prelude.Some(KeyValuePair.Create("origin", Origin.ToString())),
            TravelDates.Map(dates => KeyValuePair.Create("departureDate", dates.ToString())),
            OneWay.Map(oneWay => KeyValuePair.Create("oneWay", oneWay.ToString().ToLowerInvariant())),
            TripDurationDays.Map(duration => KeyValuePair.Create("duration", duration.ToString(CultureInfo.InvariantCulture))),
            NonStop.Map(nonStop => KeyValuePair.Create("nonStop", nonStop.ToString().ToLowerInvariant())),
            MaxPrice.Map(price => KeyValuePair.Create("maxPrice", price.ToString(CultureInfo.InvariantCulture))))
        .Choose(option => option);
}
