using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Analytics(
    [property: JsonPropertyName("travelers")] Travelers? Travelers);

