using System.ComponentModel.DataAnnotations;

namespace Amadeus.Net.Options;

public sealed record AmadeusCredentials(
    [Required] string ApiKey,
    [Required] string ApiSecret);
