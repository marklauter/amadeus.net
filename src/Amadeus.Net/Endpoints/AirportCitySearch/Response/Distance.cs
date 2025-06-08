using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Distance(
    [property: JsonPropertyName("value")] int? Value,
    [property: JsonPropertyName("unit")] DistanceUnit? Unit);
