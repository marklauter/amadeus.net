using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Warning(
    [property: JsonPropertyName("status")] int? Status,
    [property: JsonPropertyName("code")] int? Code,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("detail")] string? Detail,
    [property: JsonPropertyName("source")] WarningSource? Source
);
