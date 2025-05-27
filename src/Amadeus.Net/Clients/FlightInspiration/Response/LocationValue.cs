using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Response;

public sealed record LocationValue(
    [property: JsonPropertyName("subType")] string SubType,
    [property: JsonPropertyName("detailedName")] string DetailedName
);

