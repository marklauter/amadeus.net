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
    : IEndpointFactory<AirportCitySearchResponse, AirportCitySearchFilter>
{
    private const string Path = "/v1/reference-data/locations";

    public Endpoint<AirportCitySearchResponse, AirportCitySearchFilter> CreateEndpoint() =>
        new(TryGetLocationsAsync);

    internal async Task<Either<ErrorResponse, AirportCitySearchResponse>> TryGetLocationsAsync(
        AirportCitySearchFilter filter,
        CancellationToken cancellationToken)
    {
        var queryParams = filter.AsQueryParams().ToArray();

        using var request = BuildRequest(
            HttpMethod.Get,
            Path,
            queryParams);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<AirportCitySearchResponse>(cancellationToken);
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        params KeyValuePair<string, string>[] query) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}

