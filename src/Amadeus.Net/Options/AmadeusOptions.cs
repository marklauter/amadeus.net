using System.ComponentModel.DataAnnotations;

namespace Amadeus.Net.Options;

public sealed record AmadeusOptions(
    [Required] Uri Host,
    [Required] Version ClientVersion,
    [Required] string ClientName)
{
    public const string SectionName = "Amadeus";
}
