using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Response metadata information
/// </summary>
public sealed record Meta(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("links")] Links? Links);
