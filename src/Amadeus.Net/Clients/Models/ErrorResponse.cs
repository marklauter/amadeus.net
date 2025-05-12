using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.Models;

/// <summary>
/// Error response
/// </summary>
public sealed record ErrorResponse(
    [property: JsonPropertyName("errors")] IEnumerable<ApiError> Errors)
{
    public static ErrorResponse Create(params ApiError[] errors) => new(errors);
}
