using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record LocationValue(
    [property: JsonPropertyName("subType")] string SubType,
    [property: JsonPropertyName("detailedName")] string DetailedName
);

