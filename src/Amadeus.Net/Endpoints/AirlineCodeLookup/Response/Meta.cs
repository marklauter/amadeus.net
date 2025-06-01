using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineCodeLookup.Response;

/// <summary>
/// Response metadata information
/// </summary>
public sealed record Meta(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("links")] Links? Links);
