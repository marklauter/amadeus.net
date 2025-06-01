using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record Defaults(
    [property: JsonPropertyName("departureDate")] string? DepartureDate = null,
    [property: JsonPropertyName("oneWay")] bool? OneWay = null,
    [property: JsonPropertyName("duration")] string? Duration = null,
    [property: JsonPropertyName("nonStop")] bool? NonStop = null,
    [property: JsonPropertyName("viewBy")] string? ViewBy = null
);

