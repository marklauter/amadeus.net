using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Location(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("self")] Links? Self,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("subType")] string SubType,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("detailedName")] string? DetailedName,
    [property: JsonPropertyName("timeZoneOffset")] string? TimeZoneOffset,
    [property: JsonPropertyName("iataCode")] string? IataCode,
    [property: JsonPropertyName("geoCode")] GeoCode? GeoCode,
    [property: JsonPropertyName("address")] Address? Address,
    [property: JsonPropertyName("distance")] Distance? Distance,
    [property: JsonPropertyName("analytics")] Analytics? Analytics,
    [property: JsonPropertyName("relevance")] double? Relevance,
    [property: JsonPropertyName("category")] string? Category,
    [property: JsonPropertyName("tags")] IReadOnlyList<string>? Tags,
    [property: JsonPropertyName("rank")] string? Rank);
