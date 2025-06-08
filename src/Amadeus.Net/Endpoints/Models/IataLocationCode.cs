namespace Amadeus.Net.Endpoints.Models;

/// <summary>
/// Three digit code for city or airport. 
/// Ex: TPA = Tampa, Tampa Intl. Airport
/// Ex: TPA = Tampa, Metropolitan Area
/// </summary>
public readonly record struct IataLocationCode
{
    private readonly string value;

    private IataLocationCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        if (code.Length != 3)
            throw new ArgumentException("IATA code must be exactly 3 letters.", nameof(code));

        value = code.ToUpperInvariant();
    }

    public override string ToString() => value;
    public static implicit operator string(IataLocationCode code) => code.value;
    public static implicit operator IataLocationCode(string code) => new(code);
}
