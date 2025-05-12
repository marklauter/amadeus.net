namespace Amadeus.Net.Clients.FlightInspiration;

public sealed record IataCode
{
    private readonly string value;

    private IataCode(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentNullException(nameof(code));
        if (code.Length != 3)
            throw new ArgumentException("IATA code must be exactly 3 letters.", nameof(code));

        value = code.ToUpperInvariant();
    }

    public override string ToString() => value;
    public static implicit operator string(IataCode code) => code.value;
    public static explicit operator IataCode(string code) => new(code);
}
