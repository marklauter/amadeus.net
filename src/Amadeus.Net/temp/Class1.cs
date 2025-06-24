using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amadeus.Net.temp;

[JsonConverter(typeof(MyTypeJsonConverter))] // Apply the custom converter
public readonly record struct MyType
{
    private readonly string value;

    // This constructor is now primarily used internally via the implicit operator
    // or by the custom converter if it were to call it directly (though it uses the operator here).
    // The [JsonConstructor] attribute is less relevant for direct string deserialization
    // when a custom converter for the type is active.
    [JsonConstructor]
    private MyType(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    public static implicit operator MyType(string value) => new(value);
    public static implicit operator string(MyType value) => value.value;
}

public class MyTypeJsonConverter : JsonConverter<MyType>
{
    public override MyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            // We can use the public implicit conversion operator,
            // which in turn calls the private constructor.
            // Ensure stringValue is not null before conversion if your implicit operator doesn't handle it,
            // though your constructor already throws for null/whitespace.
            if (stringValue is not null)
            {
                return (MyType)stringValue;
            }
        }

        throw new JsonException($"Expected a JSON string to deserialize to MyType, but got {reader.TokenType}.");
    }

    public override void Write(Utf8JsonWriter writer, MyType value, JsonSerializerOptions options) =>
        // Use the public implicit conversion operator to get the string.
        writer.WriteStringValue((string)value);
}
