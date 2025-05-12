using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.FlightInspiration.Models;

public sealed record FlightDestination(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("origin")] string Origin,
    [property: JsonPropertyName("destination")] string Destination,
    [property: JsonPropertyName("departureDate")] string DepartureDate,
    [property: JsonPropertyName("returnDate")] string? ReturnDate = null,
    [property: JsonPropertyName("price")] Price? Price = null,
    [property: JsonPropertyName("links")] FlightLinks? Links = null
);
