using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.Response;

/// <summary>
/// Source of an error or warning
/// </summary>
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "doesn't matter")]
public sealed record Source(
    [property: JsonPropertyName("pointer")] string? Pointer,
    [property: JsonPropertyName("parameter")] string? Parameter,
    [property: JsonPropertyName("example")] string? Example);
