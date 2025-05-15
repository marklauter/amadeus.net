using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public sealed record AirlineCodeFilter(Option<IEnumerable<string>> Codes)
{
    public static AirlineCodeFilter Some(params string[] codes) =>
        new(codes);

    public static AirlineCodeFilter None() =>
        new(Option<IEnumerable<string>>.None);
}
