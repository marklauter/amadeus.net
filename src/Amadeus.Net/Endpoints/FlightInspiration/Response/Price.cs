using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record Price(
    [property: JsonPropertyName("total")] string Total
);

