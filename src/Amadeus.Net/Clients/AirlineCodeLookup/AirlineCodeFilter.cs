using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public class AirlineCodeFilter
{
    public IEnumerable<string> Codes { get; }

    private AirlineCodeFilter(string[] codes) => Codes = codes;

    public static Option<AirlineCodeFilter> Some(params string[] codes) =>
        new AirlineCodeFilter(codes);

    public static Option<AirlineCodeFilter> None() =>
        Option<AirlineCodeFilter>.None;
}
