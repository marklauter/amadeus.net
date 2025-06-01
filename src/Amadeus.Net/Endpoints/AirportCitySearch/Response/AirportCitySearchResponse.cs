using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record AirportCitySearchResponse(
    [property: JsonPropertyName("meta")] Meta? Meta,
    [property: JsonPropertyName("data")] IReadOnlyList<Location> Locations,
    [property: JsonPropertyName("warnings")] IReadOnlyList<Warning>? Warnings,
    [property: JsonPropertyName("links")] Links? Links);

