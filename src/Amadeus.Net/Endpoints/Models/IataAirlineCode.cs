namespace Amadeus.Net.Endpoints.Models;

public readonly record struct IataAirlineCode
{
    private readonly string value;

    private IataAirlineCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        if (code.Length != 2)
            throw new ArgumentException("IATA code must be exactly 3 letters.", nameof(code));

        value = code.ToUpperInvariant();
    }

    public override string ToString() => value;
    public static implicit operator string(IataAirlineCode code) => code.value;
    public static implicit operator IataAirlineCode(string code) => new(code);
}
