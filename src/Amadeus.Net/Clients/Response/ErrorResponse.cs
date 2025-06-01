using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.Response;

public sealed record ErrorResponse(
    [property: JsonPropertyName("errors")] IEnumerable<ApiError> Errors)
{
    public static ErrorResponse Create(params ApiError[] errors) => new(errors);

    public override string ToString() => string.Join(", ", Errors.Select(e => e.ToString()));
}
