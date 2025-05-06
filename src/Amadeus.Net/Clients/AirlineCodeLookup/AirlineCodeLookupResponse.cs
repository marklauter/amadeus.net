using Amadeus.Net.Clients.AirlineCodeLookup.Models;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

public sealed class AirlineCodeLookupResponse(
    Airlines? Data,
    ErrorResponse? Error)
    : ApiResponse<Airlines, ErrorResponse>(Data, Error)
{
    public static AirlineCodeLookupResponse Create((Airlines? data, ErrorResponse? error) response) =>
        new(response.data, response.error);
}
