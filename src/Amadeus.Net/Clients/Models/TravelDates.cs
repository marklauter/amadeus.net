using LanguageExt;

namespace Amadeus.Net.Clients.Models;

public sealed record TravelDates(
    DateOnly Departure,
    Option<DateOnly> Return)
{
    public override string ToString() => Return.Match(
        Some: r => $"{Departure:yyyy-MM-dd},{r:yyyy-MM-dd}",
        None: $"{Departure:yyyy-MM-dd}");
}
