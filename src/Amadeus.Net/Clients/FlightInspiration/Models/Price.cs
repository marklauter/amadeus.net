using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record Price(
    [property: JsonPropertyName("total")] string Total
);

