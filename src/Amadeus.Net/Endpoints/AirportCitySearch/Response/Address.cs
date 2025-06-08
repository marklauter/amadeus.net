using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Address(
    [property: JsonPropertyName("cityName")] string? CityName,
    [property: JsonPropertyName("cityCode")] string? CityCode,
    [property: JsonPropertyName("countryName")] string? CountryName,
    [property: JsonPropertyName("countryCode")] string? CountryCode,
    [property: JsonPropertyName("stateCode")] string? StateCode,
    [property: JsonPropertyName("regionCode")] string? RegionCode);
