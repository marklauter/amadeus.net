using Amadeus.Net.Clients.AirportCitySearch.Response;
using LanguageExt;
using System.Globalization;

namespace Amadeus.Net.Clients.AirportCitySearch;

public sealed record AirportCityQuery(
    Seq<LocationType> MetaLocations,
    string Keyword,
    // Country code of the location using [ISO 3166-1 alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2) code format (e.g. US).
    Option<string> CountryCode,
    Option<int> PageLimit,
    Option<int> PageOffset,
    bool Sorted,
    Option<ViewType> View)
    : IQuery
{
    public static AirportCityQuery StartsWith(string keyword) =>
        new([], keyword, Option<string>.None, Option<int>.None, Option<int>.None, false, Option<ViewType>.None);
    public AirportCityQuery WithCountryCode(string code) => this with { CountryCode = code };

    public AirportCityQuery IncludeAirports() => IncludeLocationType(LocationType.Airport);
    public AirportCityQuery IncludeCities() => IncludeLocationType(LocationType.City);
    public AirportCityQuery IncludePointsOfInterest() => IncludeLocationType(LocationType.PointOfInterest);
    public AirportCityQuery IncludeDistricts() => IncludeLocationType(LocationType.District);
    public AirportCityQuery IncludeAllMetaLocations() => IncludeAirports().IncludeCities().IncludePointsOfInterest().IncludeDistricts();
    private AirportCityQuery IncludeLocationType(LocationType location) => this with { MetaLocations = MetaLocations.Add(location) };

    public AirportCityQuery Take(int take) => this with { PageLimit = take };
    public AirportCityQuery Skip(int skip) => this with { PageOffset = skip };
    public AirportCityQuery Sort() => this with { Sorted = true };
    public AirportCityQuery WithView(ViewType view) => this with { View = view };

    public Seq<KeyValuePair<string, string>> ToParams() =>
        Prelude.Seq(
            Prelude.Some(KeyValuePair.Create("subType", string.Join(",", MetaLocations.Distinct().Select(s => s.ToEnumMemberString())))),
            Prelude.Some(KeyValuePair.Create("keyword", Keyword)),
            CountryCode.Map(code => KeyValuePair.Create("countryCode", code)),
            PageLimit.Map(limit => KeyValuePair.Create("page[limit]", limit.ToString(CultureInfo.InvariantCulture))),
            PageOffset.Map(offset => KeyValuePair.Create("page[offset]", offset.ToString(CultureInfo.InvariantCulture))),
            Sorted ? Prelude.Some(KeyValuePair.Create("sort", "analytics.travelers.score")) : Prelude.None,
            View.Map(viewType => KeyValuePair.Create("view", viewType.ToString().ToUpperInvariant())))
        .Choose(option => option);
}

