using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocationCategory
{
    [EnumMember(Value = "SIGHTS")]
    Sights,
    [EnumMember(Value = "BEACH_PARK")]
    BeachPark,
    [EnumMember(Value = "HISTORICAL")]
    Historical,
    [EnumMember(Value = "NIGHTLIFE")]
    Nightlife,
    [EnumMember(Value = "RESTAURANT")]
    Restaurant,
    [EnumMember(Value = "SHOPPING")]
    Shopping
}
