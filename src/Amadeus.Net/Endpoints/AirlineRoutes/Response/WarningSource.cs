// Amadeus.Net\Endpoints\AirlineRoutes\Response\WarningSource.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record WarningSource(
    [property: JsonPropertyName("parameter")] string? Parameter = null,
    [property: JsonPropertyName("pointer")] string? Pointer = null,
    [property: JsonPropertyName("example")] string? Example = null
);