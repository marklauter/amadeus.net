using Amadeus.Net.Clients.Models;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Response;

public sealed record AirportCitySearchResponse(
    [property: JsonPropertyName("meta")] Meta? Meta,
    [property: JsonPropertyName("data")] IReadOnlyList<Location> Data,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Warning>? Warnings,
    [property: JsonPropertyName("links")] Links? Links
);

