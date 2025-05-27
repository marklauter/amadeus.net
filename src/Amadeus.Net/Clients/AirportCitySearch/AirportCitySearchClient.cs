using Amadeus.Net.Clients.AirportCitySearch.Response;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

internal sealed class AirportCitySearchClient(
    HttpClient httpClient,
    AmadeusOptions options)
//: IEndpointFactory<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>>
{
    private const string Path = "/v1/reference-data/locations";

    //public Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>> CreateEndpoint() =>
    //    new(TryGetLocationsAsync);

    //.Map(r => r.Map<Either<AirportCitySearchResponse, Location>>(r => r))

    internal IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(
        Either<AirportCitySearchFilter, LocationId> filterOrLocationId) =>
        filterOrLocationId.Match(
            Left: Filter,
            Right: Filter);

    private IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(AirportCitySearchFilter filter) =>
        MapResult(Prelude.use(
            acquire: () => BuildRequest(HttpMethod.Get, Path, filter.AsQuery()),
            release: request => request.Dispose())
            .Bind(ReadLocations<AirportCitySearchResponse>));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, AirportCitySearchResponse>> responseIO) =>
        responseIO.Map(eitherResponse =>
            eitherResponse.Map<Either<AirportCitySearchResponse, Location>>(searchResponse =>
                searchResponse));

    private IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> Filter(LocationId locationId) =>
        MapResult(Prelude.use(
            acquire: () => BuildRequest(HttpMethod.Get, $"{Path}/{locationId}", []),
            release: request => request.Dispose())
            .Bind(ReadLocations<Location>));

    private static IO<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> MapResult(
        IO<Either<ErrorResponse, Location>> locationIO) =>
        locationIO.Map(eitherLocation =>
            eitherLocation.Map<Either<AirportCitySearchResponse, Location>>(location =>
                location));

    //private IO<Either<ErrorResponse, AirportCitySearchResponse>> Filter(AirportCitySearchFilter filter) =>
    //    Prelude.use(
    //        acquire: () => BuildRequest(HttpMethod.Get, Path, filter.AsQuery()),
    //        release: request => request.Dispose())
    //        .Bind(ReadLocations<AirportCitySearchResponse>);

    //private IO<Either<ErrorResponse, Location>> Filter(LocationId locationId) =>
    //    Prelude.use(
    //        acquire: () => BuildRequest(HttpMethod.Get, $"{Path}/{locationId}", []),
    //        release: request => request.Dispose())
    //        .Bind(ReadLocations<Location>);

    private IO<Either<ErrorResponse, T>> ReadLocations<T>(HttpRequestMessage request) =>
        Prelude.use(
            acquire: httpClient.SendIO(request),
            release: response => response.Dispose())
            .Bind(response => response.Parse<T>());

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
