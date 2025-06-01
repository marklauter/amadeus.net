using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ViewType
{
    [EnumMember(Value = "LIGHT")]
    Light,
    [EnumMember(Value = "FULL")]
    Full
}
