using Amadeus.Net.Clients.AirportCitySearch.Models;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using System.Globalization;

namespace Amadeus.Net.Clients.AirportCitySearch;

public sealed record AirportCitySearchFilter(
    IReadOnlyList<LocationSubType> SubType,
    string Keyword,
    Option<string> CountryCode,
    Option<int> PageLimit,
    Option<int> PageOffset,
    Option<SortType> Sort,
    Option<ViewType> View)
{
    public static AirportCitySearchFilter Create(
        IReadOnlyList<LocationSubType> subType,
        string keyword) =>
        new(subType, keyword, Option<string>.None, Option<int>.None, Option<int>.None, Option<SortType>.None, Option<ViewType>.None);

    public IEnumerable<KeyValuePair<string, string>> AsQueryParams()
    {
        yield return KeyValuePair.Create("subType", string.Join(",", SubType.Select(s => s.ToString().ToUpperInvariant())));
        yield return KeyValuePair.Create("keyword", Keyword);
        if (CountryCode.IsSome)
            yield return KeyValuePair.Create("countryCode", CountryCode.ValueUnsafe());
        if (PageLimit.IsSome)
            yield return KeyValuePair.Create("page[limit]", PageLimit.ValueUnsafe().ToString(CultureInfo.InvariantCulture));
        if (PageOffset.IsSome)
            yield return KeyValuePair.Create("page[offset]", PageOffset.ValueUnsafe().ToString(CultureInfo.InvariantCulture));
        if (Sort.IsSome)
            yield return KeyValuePair.Create("sort", Sort.ValueUnsafe().ToString().Replace("_", ".").ToLowerInvariant());
        if (View.IsSome)
            yield return KeyValuePair.Create("view", View.ValueUnsafe().ToString().ToUpperInvariant());
    }
}

