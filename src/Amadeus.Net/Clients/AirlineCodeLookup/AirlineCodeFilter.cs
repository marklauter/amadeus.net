using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public sealed record AirlineCodeFilter(IEnumerable<string> Codes)
{
    public static Option<AirlineCodeFilter> Some(params string[] codes) =>
        new AirlineCodeFilter(codes);

    public static Option<AirlineCodeFilter> None() =>
        Option<AirlineCodeFilter>.None;
}
