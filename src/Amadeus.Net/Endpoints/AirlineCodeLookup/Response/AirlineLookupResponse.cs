using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineCodeLookup.Response;

/// <summary>
/// Response containing airline information
/// </summary>
public sealed record AirlineLookupResponse(
    [property: JsonPropertyName("meta")] Meta? Meta,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Warning>? Warnings,
    [property: JsonPropertyName("data")] IReadOnlyList<Airline> Airlines);
