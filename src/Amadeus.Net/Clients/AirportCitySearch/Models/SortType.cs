using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Amadeus.Net.Clients.AirportCitySearch.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortType
{
    [EnumMember(Value = "ANALYTICS_TRAVELERS_SCORE")]
    AnalyticsTravelersScore
}
