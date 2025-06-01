using System.ComponentModel.DataAnnotations;

namespace Amadeus.Net.Options;

public sealed record ClientMetaData(
    [Required] Version ClientVersion,
    [Required] string ClientName);
