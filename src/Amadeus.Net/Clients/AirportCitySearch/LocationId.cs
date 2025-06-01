using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

public sealed class LocationId
    : IQuery
{
    private readonly string value;

    private LocationId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.ToUpperInvariant();
    }

    public Seq<QueryParameter> ToParams() => [];

    public override string ToString() => value;
    public static implicit operator string(LocationId code) => code.value;
    public static implicit operator LocationId(string code) => new(code);
}
