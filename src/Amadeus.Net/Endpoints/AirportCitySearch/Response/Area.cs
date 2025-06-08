using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Area(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("code")] string? Code,
    [property: JsonPropertyName("locationCode")] string? LocationCode
);
