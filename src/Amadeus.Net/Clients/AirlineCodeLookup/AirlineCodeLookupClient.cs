using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Clients.LINQ;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

internal sealed class AirlineCodeLookupClient(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private const string Path = "/v1/reference-data/airlines";

    internal async Task<Either<ErrorResponse, Airlines>> TryGetAirlinesByCodesAsync(
        Option<IEnumerable<string>> airlineCodes,
        CancellationToken cancellationToken) =>
        await airlineCodes
            .MatchAsync(
                Some: async codes =>
                {
                    using var request = BuildRequest(
                        HttpMethod.Get,
                        Path,
                        KeyValuePair.Create("airlineCodes", string.Join(',', codes.Distinct())));
                    return await SendAsync(request, cancellationToken);
                },
                None: async () =>
                {
                    using var request = BuildRequest(HttpMethod.Get, Path);
                    return await SendAsync(request, cancellationToken);
                });

    internal Task<Either<ErrorResponse, Airlines>> TryGetAllAirlinesAsync(CancellationToken cancellationToken) =>
        TryGetAirlinesByCodesAsync(Option<IEnumerable<string>>.None, cancellationToken);

    private async Task<Either<ErrorResponse, Airlines>> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<Airlines>(cancellationToken);
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        params KeyValuePair<string, string>[] query) =>
        new HttpRequestMessageBuilder(method, path)
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
