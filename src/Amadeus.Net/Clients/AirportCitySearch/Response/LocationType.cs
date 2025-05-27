using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Response;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocationType
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
