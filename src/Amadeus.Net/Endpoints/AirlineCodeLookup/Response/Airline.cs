using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirlineCodeLookup.Response;

/// <summary>
/// Represents an airline company with IATA and ICAO codes
/// </summary>
public sealed record Airline(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("iataCode")] string IataCode,
    [property: JsonPropertyName("icaoCode")] string IcaoCode,
    [property: JsonPropertyName("businessName")] string BusinessName,
    [property: JsonPropertyName("commonName")] string CommonName);
