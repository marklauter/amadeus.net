using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.FlightInspiration.Response;

public sealed record FlightLinks(
    [property: JsonPropertyName("flightDates")] string? FlightDates = null,
    [property: JsonPropertyName("flightOffers")] string? FlightOffers = null
);

