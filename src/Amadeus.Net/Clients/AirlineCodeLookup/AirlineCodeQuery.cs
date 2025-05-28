using LanguageExt;
using LanguageExt.Traits;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public sealed record AirlineCodeQuery(Seq<string> Codes)
    : Monoid<AirlineCodeQuery>
    , IQuery
{
    public static AirlineCodeQuery Empty { get; } =
        new([]);

    public static AirlineCodeQuery From(string code) =>
        From(Prelude.Seq(code));

    public static AirlineCodeQuery From(Seq<string> codes) =>
        new(codes);

    public AirlineCodeQuery Add(string code) =>
        new(Codes.Add(code));

    public AirlineCodeQuery Combine(AirlineCodeQuery rhs) =>
        new(Codes.Combine(rhs.Codes));

    public Seq<KeyValuePair<string, string>> ToParams() =>
        Codes.Match(
            Empty: () => [],
            Seq: codes => Prelude.Seq(KeyValuePair.Create("airlineCodes", string.Join(',', codes.Distinct()))));
}
