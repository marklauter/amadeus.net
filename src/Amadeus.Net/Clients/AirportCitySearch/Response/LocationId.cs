using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch.Response;

public sealed class LocationId
    : IFilter
{
    private readonly string value;

    private LocationId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.ToUpperInvariant();
    }

    public Seq<KeyValuePair<string, string>> AsQuery() => [];

    public override string ToString() => value;
    public static implicit operator string(LocationId code) => code.value;
    public static implicit operator LocationId(string code) => new(code);
}
