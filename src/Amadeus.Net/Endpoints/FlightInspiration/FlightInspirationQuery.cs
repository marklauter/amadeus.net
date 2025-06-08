using Amadeus.Net.Endpoints.Models;
using Amadeus.Net.Endpoints.Query;
using LanguageExt;
using System.Globalization;

namespace Amadeus.Net.Endpoints.FlightInspiration;

public sealed record FlightInspirationQuery(
    IataLocationCode Origin,
    Option<TravelDates> TravelDates,
    Option<bool> OneWay,
    Option<int> TripDurationDays,
    Option<bool> NonStop,
    Option<int> MaxPrice)
    : IQuery
{
    public static FlightInspirationQuery From(IataLocationCode origin) => new(
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

    public Seq<QueryParameter> ToParams() =>
        Prelude.Seq(
            Prelude.Some(QueryParameter.Create("origin", Origin.ToString())),
            TravelDates.Map(dates => QueryParameter.Create("departureDate", dates.ToString())),
            OneWay.Map(oneWay => QueryParameter.Create("oneWay", oneWay.ToString().ToLowerInvariant())),
            TripDurationDays.Map(duration => QueryParameter.Create("duration", duration.ToString(CultureInfo.InvariantCulture))),
            NonStop.Map(nonStop => QueryParameter.Create("nonStop", nonStop.ToString().ToLowerInvariant())),
            MaxPrice.Map(price => QueryParameter.Create("maxPrice", price.ToString(CultureInfo.InvariantCulture))))
        .Choose(option => option);
}
