using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record WarningSource(
    [property: JsonPropertyName("parameter")] string? Parameter,
    [property: JsonPropertyName("example")] string? Example
);
