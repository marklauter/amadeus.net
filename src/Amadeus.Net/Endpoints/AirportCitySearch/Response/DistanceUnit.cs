using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DistanceUnit
{
    [EnumMember(Value = "KM")]
    Kil,
    [EnumMember(Value = "MI")]
    Miles
}
