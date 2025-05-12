using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Response containing airline information
/// </summary>
public sealed record Airlines(
    [property: JsonPropertyName("meta")] Meta? Meta,
    [property: JsonPropertyName("warnings")] IEnumerable<Warning> Warnings,
    [property: JsonPropertyName("data")] IEnumerable<Airline> Data);
