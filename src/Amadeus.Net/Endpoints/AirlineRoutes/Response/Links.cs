// Amadeus.Net\Endpoints\AirlineRoutes\Response\Links.cs
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineRoutes.Response;

public sealed record Links(
    [property: JsonPropertyName("self")] string Self
);