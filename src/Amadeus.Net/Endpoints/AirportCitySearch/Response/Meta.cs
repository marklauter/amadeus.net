using Amadeus.Net.Endpoints.Response;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

public sealed record Meta(
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("links")] Links? Links);

