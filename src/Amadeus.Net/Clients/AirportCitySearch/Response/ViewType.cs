using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Response;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ViewType
{
    [EnumMember(Value = "LIGHT")]
    Light,
    [EnumMember(Value = "FULL")]
    Full
}
