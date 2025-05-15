using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.AirportCitySearch.Models;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using Amadeus.Net.Responses;
using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

internal sealed class AirportCitySearchClient(
    HttpClient httpClient,
    AmadeusOptions options)
    : IEndpointFactory<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>>
{
    private const string Path = "/v1/reference-data/locations";

    public Endpoint<Either<AirportCitySearchResponse, Location>, Either<AirportCitySearchFilter, LocationId>> CreateEndpoint() =>
        new(TryGetLocationsAsync);

    internal async Task<Either<ErrorResponse, Either<AirportCitySearchResponse, Location>>> TryGetLocationsAsync(
        Either<AirportCitySearchFilter, LocationId> filter,
        CancellationToken cancellationToken) =>
        await filter.Match(
            async filter => (await TryGetLocationsAsync(filter, cancellationToken))
                .Map<Either<AirportCitySearchResponse, Location>>(response => response),
            async id => (await TryGetLocationsAsync(id, cancellationToken))
                .Map<Either<AirportCitySearchResponse, Location>>(location => location)
        );

    private async Task<Either<ErrorResponse, AirportCitySearchResponse>> TryGetLocationsAsync(
        AirportCitySearchFilter filter,
        CancellationToken cancellationToken)
    {
        using var request = BuildRequest(HttpMethod.Get, Path, filter.AsQueryParams().ToOption());
        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<AirportCitySearchResponse>(cancellationToken);
    }

    internal async Task<Either<ErrorResponse, Location>> TryGetLocationsAsync(
        LocationId locationId,
        CancellationToken cancellationToken)
    {
        var path = $"{Path}/{locationId}";
        using var request = BuildRequest(HttpMethod.Get, path, Option<IEnumerable<KeyValuePair<string, string>>>.None);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<Location>(cancellationToken);
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        Option<IEnumerable<KeyValuePair<string, string>>> query) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
