using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Response;

public sealed record Dictionaries(
    [property: JsonPropertyName("currencies")] Dictionary<string, string>? Currencies = null,
    [property: JsonPropertyName("locations")] Dictionary<string, LocationValue>? Locations = null
);

