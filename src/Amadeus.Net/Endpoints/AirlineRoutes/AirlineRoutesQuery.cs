// Amadeus.Net\Endpoints\AirlineRoutes\AirlineRoutesQuery.cs
using Amadeus.Net.Endpoints.Models;
using Amadeus.Net.Endpoints.Query;
using LanguageExt;
using System.Globalization;

namespace Amadeus.Net.Endpoints.AirlineRoutes;

public sealed record AirlineRoutesQuery(
    IataAirlineCode AirlineCode,
    Option<int> Max,
    Option<string> ArrivalCountryCode)
    : IQuery
{
    public static AirlineRoutesQuery Create(IataAirlineCode airlineCode) => new(
        airlineCode,
        Option<int>.None,
        Option<string>.None);

    public AirlineRoutesQuery WithMax(int max) => this with { Max = max };
    public AirlineRoutesQuery WithArrivalCountryCode(string countryCode) => this with { ArrivalCountryCode = countryCode };

    public Seq<QueryParameter> ToParams() =>
        Prelude.Seq(
            Prelude.Some(QueryParameter.Create("airlineCode", AirlineCode.ToString())),
            Max.Map(max => QueryParameter.Create("max", max.ToString(CultureInfo.InvariantCulture))),
            ArrivalCountryCode.Map(code => QueryParameter.Create("arrivalCountryCode", code)))
        .Choose(option => option);
}
