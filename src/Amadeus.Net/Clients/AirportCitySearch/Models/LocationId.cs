namespace Amadeus.Net.Clients.AirportCitySearch.Models;

public readonly struct LocationId
{
    private readonly string value;

    private LocationId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.ToUpperInvariant();
    }

    public override string ToString() => value;
    public static implicit operator string(LocationId code) => code.value;
    public static implicit operator LocationId(string code) => new(code);
}
