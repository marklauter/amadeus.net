using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using Amadeus.Net.HttpClientExtensions;
using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

internal sealed class AirlineCodeLookupClient(
    HttpClient httpClient,
    AmadeusOptions options)
    : IEndpointFactory<AirlineLookupResponse, AirlineCodeFilter>
{
    private const string Path = "/v1/reference-data/airlines";

    public Endpoint<AirlineLookupResponse, AirlineCodeFilter> CreateEndpoint() =>
        new(TryGetAirlinesByCodesAsync);

    internal async Task<Either<ErrorResponse, AirlineLookupResponse>> TryGetAirlinesByCodesAsync(
        AirlineCodeFilter filter,
        CancellationToken cancellationToken) =>
        await filter.Codes.Match(
            Empty: () =>
            {
                using var request = BuildRequest(HttpMethod.Get, Path);
                return SendAsync(request, cancellationToken);
            },
            Seq: (codes) =>
            {
                using var request = BuildRequest(
                    HttpMethod.Get,
                    Path,
                    KeyValuePair.Create("airlineCodes", string.Join(',', codes.Distinct())));
                return SendAsync(request, cancellationToken);
            });

    private async Task<Either<ErrorResponse, AirlineLookupResponse>> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<AirlineLookupResponse>(cancellationToken);
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
