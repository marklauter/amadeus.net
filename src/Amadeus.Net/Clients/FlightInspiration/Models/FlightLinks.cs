using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record FlightLinks(
    [property: JsonPropertyName("flightDates")] string? FlightDates = null,
    [property: JsonPropertyName("flightOffers")] string? FlightOffers = null
);

