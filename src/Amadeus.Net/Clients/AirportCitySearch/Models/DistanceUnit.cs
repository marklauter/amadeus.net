using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DistanceUnit
{
    [EnumMember(Value = "KM")]
    Kil,
    [EnumMember(Value = "MI")]
    Miles
}
