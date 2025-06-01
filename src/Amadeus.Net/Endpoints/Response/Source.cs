using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.Response;

/// <summary>
/// Source of an error or warning
/// </summary>
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "doesn't matter")]
public sealed record Source(
    [property: JsonPropertyName("pointer")] string? Pointer,
    [property: JsonPropertyName("parameter")] string? Parameter,
    [property: JsonPropertyName("example")] string? Example)
{
    public override string ToString() => $"{Pointer} {Parameter} {Example}";
}
