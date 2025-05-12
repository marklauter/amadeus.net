using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record Links(
    [property: JsonPropertyName("self")] string? Self = null
);

