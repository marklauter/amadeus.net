using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup.Models;

/// <summary>
/// Error response
/// </summary>
public sealed record ErrorResponse(
    [property: JsonPropertyName("errors")] IEnumerable<Error> Errors);

