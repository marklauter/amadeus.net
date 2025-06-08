using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Travelers(
    [property: JsonPropertyName("score")] int? Score);

