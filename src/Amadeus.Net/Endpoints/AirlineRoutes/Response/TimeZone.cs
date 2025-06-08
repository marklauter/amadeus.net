// Amadeus.Net\Endpoints\AirlineRoutes\Response\TimeZone.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record TimeZone(
    [property: JsonPropertyName("offset")] string Offset,
    [property: JsonPropertyName("referenceLocalDateTime")] string ReferenceLocalDateTime
);