using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record Issue(
    [property: JsonPropertyName("status")] string? Status = null,
    [property: JsonPropertyName("code")] string? Code = null,
    [property: JsonPropertyName("title")] string? Title = null,
    [property: JsonPropertyName("detail")] string? Detail = null
);

