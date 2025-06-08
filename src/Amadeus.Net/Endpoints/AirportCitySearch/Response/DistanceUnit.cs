using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DistanceUnit
{
    [EnumMember(Value = "KM")]
    Kilometers,
    [EnumMember(Value = "MI")]
    Miles
}
