using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Response;

public sealed record GeoCode(
    [property: JsonPropertyName("latitude")] double? Latitude,
    [property: JsonPropertyName("longitude")] double? Longitude
);
