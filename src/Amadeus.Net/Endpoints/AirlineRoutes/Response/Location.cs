// Amadeus.Net\Endpoints\AirlineRoutes\Response\Location.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record Location(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("subtype")] string Subtype,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("iataCode")] string IataCode,
    [property: JsonPropertyName("geoCode")] GeoCode GeoCode,
    [property: JsonPropertyName("address")] Address Address,
    [property: JsonPropertyName("timeZone")] TimeZone TimeZone,
    [property: JsonPropertyName("metrics")] Metrics Metrics
);