using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record Meta(
    [property: JsonPropertyName("currency")] string? Currency = null,
    [property: JsonPropertyName("links")] Links? Links = null,
    [property: JsonPropertyName("defaults")] Defaults? Defaults = null
);

