using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

public sealed record Distance(
    [property: JsonPropertyName("value")] int? Value,
    [property: JsonPropertyName("unit")] string? Unit
);
