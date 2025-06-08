// Amadeus.Net\Endpoints\AirlineRoutes\Response\Warning.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record Warning(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("detail")] string? Detail = null,
    [property: JsonPropertyName("source")] WarningSource? Source = null
);