using Amadeus.Net.Clients.Models;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Response containing airline information
/// </summary>
public sealed record Airlines(
    [property: JsonPropertyName("meta")] Meta? Meta,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Warning>? Warnings,
    [property: JsonPropertyName("data")] IReadOnlyList<Airline> Data);
