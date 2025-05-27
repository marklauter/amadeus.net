using Amadeus.Net.Clients.AirportCitySearch.Response;
using LanguageExt;
using System.Globalization;

namespace Amadeus.Net.Clients.AirportCitySearch;

public sealed record AirportCitySearchFilter(
    Seq<LocationType> Locations,
    string StartsWith,
    // Country code of the location using [ISO 3166-1 alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2) code format (e.g. US).
    Option<string> CountryCode,
    Option<int> PageLimit,
    Option<int> PageOffset,
    bool Sorted,
    Option<ViewType> View)
    : IFilter
{
    public static AirportCitySearchFilter From(string keyword) =>
        new([], keyword, Option<string>.None, Option<int>.None, Option<int>.None, false, Option<ViewType>.None);
    public AirportCitySearchFilter WithAirports() => IncludeLocationType(LocationType.Airport);
    public AirportCitySearchFilter WithCities() => IncludeLocationType(LocationType.City);
    public AirportCitySearchFilter WithPointsOfInterest() => IncludeLocationType(LocationType.PointOfInterest);
    public AirportCitySearchFilter WithDistricts() => IncludeLocationType(LocationType.District);
    private AirportCitySearchFilter IncludeLocationType(LocationType location) => this with { Locations = Locations.Add(location) };

    public AirportCitySearchFilter WithCountryCode(string code) => this with { CountryCode = code };
    public AirportCitySearchFilter Take(int take) => this with { PageLimit = take };
    public AirportCitySearchFilter Skip(int skip) => this with { PageOffset = skip };
    public AirportCitySearchFilter Sort() => this with { Sorted = true };
    public AirportCitySearchFilter WithView(ViewType view) => this with { View = view };

    public Seq<KeyValuePair<string, string>> AsQuery() =>
        Prelude.Seq(
            Prelude.Some(KeyValuePair.Create("subType", string.Join(",", Locations.Distinct().Select(s => s.ToString().ToUpperInvariant())))),
            Prelude.Some(KeyValuePair.Create("keyword", StartsWith)),
            CountryCode.Map(code => KeyValuePair.Create("countryCode", code)),
            PageLimit.Map(limit => KeyValuePair.Create("page[limit]", limit.ToString(CultureInfo.InvariantCulture))),
            PageOffset.Map(offset => KeyValuePair.Create("page[offset]", offset.ToString(CultureInfo.InvariantCulture))),
            Sorted ? Prelude.Some(KeyValuePair.Create("sort", "analytics.travelers.score")) : Prelude.None,
            View.Map(viewType => KeyValuePair.Create("view", viewType.ToString().ToUpperInvariant())))
        .Choose(option => option);
}

