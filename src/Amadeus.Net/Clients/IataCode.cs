namespace Amadeus.Net.Clients;

public readonly record struct IataCode
{
    private readonly string value;

    private IataCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        if (code.Length != 3)
            throw new ArgumentException("IATA code must be exactly 3 letters.", nameof(code));

        value = code.ToUpperInvariant();
    }

    public override string ToString() => value;
    public static implicit operator string(IataCode code) => code.value;
    public static implicit operator IataCode(string code) => new(code);
}
