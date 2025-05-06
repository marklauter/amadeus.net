using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Source of an error or warning
/// </summary>
public sealed record Source(
    [property: JsonPropertyName("pointer")] string? Pointer,
    [property: JsonPropertyName("parameter")] string? Parameter,
    [property: JsonPropertyName("example")] string? Example);
