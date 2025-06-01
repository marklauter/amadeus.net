using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Endpoints.AirportCitySearch.Response;

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

public static class LocationTypeExtensions
{
    public static string ToEnumMemberString(this LocationType locationType) =>
        locationType switch
        {
            LocationType.Airport => "AIRPORT",
            LocationType.City => "CITY",
            LocationType.PointOfInterest => "POINT_OF_INTEREST",
            LocationType.District => "DISTRICT",
            _ => locationType.ToString()
        };
}
