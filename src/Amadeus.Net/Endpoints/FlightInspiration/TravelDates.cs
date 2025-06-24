using LanguageExt;

namespace Amadeus.Net.Endpoints.FlightInspiration;

public sealed record TravelDates(
    DateOnly Departure,
    Option<DateOnly> Return)
{
    public static TravelDates Oneway(DateOnly departureDate) => new(departureDate, Prelude.None);
    public static TravelDates RoundTrip(DateOnly departureDate, DateOnly returnDate) => new(departureDate, returnDate);

    public override string ToString() => Return.Match(
        Some: r => $"{Departure:yyyy-MM-dd},{r:yyyy-MM-dd}",
        None: () => $"{Departure:yyyy-MM-dd}");
}
