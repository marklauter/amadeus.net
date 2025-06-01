using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record FlightInspirationResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<FlightDestination> Destinations,
    [property: JsonPropertyName("dictionaries")] Dictionaries? Dictionaries = null,
    [property: JsonPropertyName("meta")] Meta? Meta = null,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Issue>? Warnings = null
);

