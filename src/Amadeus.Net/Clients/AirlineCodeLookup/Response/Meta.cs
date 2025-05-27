using Amadeus.Net.Clients.Models;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Response;

/// <summary>
/// Response metadata information
/// </summary>
public sealed record Meta(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("links")] Links? Links);
