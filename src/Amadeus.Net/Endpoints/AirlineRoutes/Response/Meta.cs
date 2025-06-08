// Amadeus.Net\Endpoints\AirlineRoutes\Response\Meta.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record Meta(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("links")] Links? Links = null
);