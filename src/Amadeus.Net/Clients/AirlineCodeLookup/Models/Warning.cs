using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Warning information
/// </summary>
public sealed record Warning(
    [property: JsonPropertyName("code")] long Code,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("detail")] string? Detail = null,
    [property: JsonPropertyName("source")] Source? Source = null);
