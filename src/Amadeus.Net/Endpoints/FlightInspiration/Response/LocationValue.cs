using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record LocationValue(
    [property: JsonPropertyName("subType")] string SubType,
    [property: JsonPropertyName("detailedName")] string DetailedName
);

