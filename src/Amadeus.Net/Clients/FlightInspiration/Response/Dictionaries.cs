using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Response;

public sealed record Dictionaries(
    [property: JsonPropertyName("currencies")] ReadOnlyDictionary<string, string>? Currencies = null,
    [property: JsonPropertyName("locations")] ReadOnlyDictionary<string, LocationValue>? Locations = null
);

