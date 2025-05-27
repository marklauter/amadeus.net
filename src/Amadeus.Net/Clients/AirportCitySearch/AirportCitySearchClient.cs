using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

internal sealed class AirportCitySearchClient(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private const string Path = "/v1/reference-data/locations";

    internal IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(
        Either<AirportCitySearchFilter, LocationId> filterOrLocationId) =>
        filterOrLocationId.Match(
            Left: Filter,
            Right: Filter);

    private IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(AirportCitySearchFilter filter) =>
        MapResult(httpClient.Filter<AirportCitySearchFilter, AirportCitySearchResponse>(options, Path, filter));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, AirportCitySearchResponse>> responseIO) =>
        responseIO.Map(eitherResponse =>
            eitherResponse.Map<Either<AirportCitySearchResponse, Location>>(searchResponse =>
                searchResponse));

    private IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(LocationId locationId) =>
        MapResult(httpClient.Filter<LocationId, Location>(options, $"{Path}/{locationId}", locationId));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, Location>> locationIO) =>
        locationIO.Map(eitherLocation =>
            eitherLocation.Map<Either<AirportCitySearchResponse, Location>>(location =>
                location));
}
