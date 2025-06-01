using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Analytics(
    [property: JsonPropertyName("travelers")] Travelers? Travelers
);

public sealed record Travelers(
    [property: JsonPropertyName("score")] int? Score
);

