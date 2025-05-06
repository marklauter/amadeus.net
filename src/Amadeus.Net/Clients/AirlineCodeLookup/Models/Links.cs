using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Collection navigation links
/// </summary>
public sealed record Links(
    [property: JsonPropertyName("self")] string Self = "",
    [property: JsonPropertyName("next")] string? Next = null,
    [property: JsonPropertyName("previous")] string? Previous = null,
    [property: JsonPropertyName("last")] string? Last = null,
    [property: JsonPropertyName("first")] string? First = null,
    [property: JsonPropertyName("up")] string? Up = null);

