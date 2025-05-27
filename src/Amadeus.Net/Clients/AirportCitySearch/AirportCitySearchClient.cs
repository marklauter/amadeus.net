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
        MapResult(Prelude.use(
            acquire: () => BuildRequest(HttpMethod.Get, Path, filter.AsQuery()),
            release: request => request.Dispose())
            .Bind(httpClient.GetIO<AirportCitySearchResponse>));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, AirportCitySearchResponse>> responseIO) =>
        responseIO.Map(eitherResponse =>
            eitherResponse.Map<Either<AirportCitySearchResponse, Location>>(searchResponse =>
                searchResponse));

    private IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(LocationId locationId) =>
        MapResult(Prelude.use(
            acquire: () => BuildRequest(HttpMethod.Get, $"{Path}/{locationId}", []),
            release: request => request.Dispose())
            .Bind(httpClient.GetIO<Location>));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, Location>> locationIO) =>
        locationIO.Map(eitherLocation =>
            eitherLocation.Map<Either<AirportCitySearchResponse, Location>>(location =>
                location));

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        Seq<KeyValuePair<string, string>> query) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
