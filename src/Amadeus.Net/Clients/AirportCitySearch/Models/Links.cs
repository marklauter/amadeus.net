using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

public sealed record Links(
    [property: JsonPropertyName("self")] string? Self,
    [property: JsonPropertyName("next")] string? Next,
    [property: JsonPropertyName("previous")] string? Previous,
    [property: JsonPropertyName("last")] string? Last,
    [property: JsonPropertyName("first")] string? First,
    [property: JsonPropertyName("up")] string? Up
);

