using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record FlightDestinations(
    [property: JsonPropertyName("data")] IReadOnlyList<FlightDestination> Data,
    [property: JsonPropertyName("dictionaries")] Dictionaries? Dictionaries = null,
    [property: JsonPropertyName("meta")] Meta? Meta = null,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Issue>? Warnings = null
);

