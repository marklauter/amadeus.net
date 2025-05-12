using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.Models;

/// <summary>
/// Error information
/// </summary>
public sealed record ApiError(
    [property: JsonPropertyName("status")] int Status,
    [property: JsonPropertyName("code")] long Code,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("detail")] string? Detail,
    [property: JsonPropertyName("source")] Source? Source);
