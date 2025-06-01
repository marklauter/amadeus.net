using System.ComponentModel.DataAnnotations;

namespace Amadeus.Net.Options;

public sealed record AmadeusOptions(
    [Required] Uri Host,
    [Required] ClientMetaData ClientMetaData)
{
    public const string SectionName = "Amadeus";
}
