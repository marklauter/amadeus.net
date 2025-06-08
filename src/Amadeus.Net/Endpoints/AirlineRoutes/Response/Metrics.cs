// Amadeus.Net\Endpoints\AirlineRoutes\Response\Metrics.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record Metrics(
    [property: JsonPropertyName("relevance")] int Relevance
);