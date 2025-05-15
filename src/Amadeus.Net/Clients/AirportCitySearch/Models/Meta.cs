using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

public sealed record Meta(
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("links")] Links? Links
);

