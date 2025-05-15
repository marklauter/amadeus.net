using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocationSubType
{
    [EnumMember(Value = "AIRPORT")]
    Airport,
    [EnumMember(Value = "CITY")]
    City,
    [EnumMember(Value = "POINT_OF_INTEREST")]
    PointOfInterest,
    [EnumMember(Value = "DISTRICT")]
    District
}
