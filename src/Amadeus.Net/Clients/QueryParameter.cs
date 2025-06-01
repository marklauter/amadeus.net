using System.ComponentModel;

namespace Amadeus.Net.Clients;

public readonly struct QueryParameter(
    string key,
    string value)
{
    public static QueryParameter Create(string key, string value) => new(key, value);
    public static QueryParameter CreateEncoded(string key, string value) => new(key, Uri.EscapeDataString(value));

    public string Key { get; } = key;
    public string Value { get; } = value;

    public override string ToString() => PairToString(Key, Value);

    internal static string PairToString(string key, string value) =>
        string.Create(null, stackalloc char[256], $"{key}={value}");

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out string key, out string value)
    {
        key = Key;
        value = Value;
    }
}
