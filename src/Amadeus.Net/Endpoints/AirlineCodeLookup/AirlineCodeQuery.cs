using Amadeus.Net.Endpoints.Query;
using LanguageExt;
using LanguageExt.Traits;

namespace Amadeus.Net.Endpoints.AirlineCodeLookup;

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

    public Seq<QueryParameter> ToParams() =>
        Codes.Match(
            Empty: () => [],
            Seq: codes => Prelude.Seq(QueryParameter.Create("airlineCodes", string.Join(',', codes.Distinct()))));
}
