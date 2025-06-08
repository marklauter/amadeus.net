// Amadeus.Net\Endpoints\AirlineRoutes\Response\AirlineRoutesResponse.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record AirlineRoutesResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<Location> Locations,
    [property: JsonPropertyName("meta")] Meta? Meta = null,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Warning>? Warnings = null
);