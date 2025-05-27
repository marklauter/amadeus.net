using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using Amadeus.Net.HttpClientExtensions;
using LanguageExt;

namespace Amadeus.Net.Clients.FlightInspiration;

internal sealed class FlightInspirationClient(
    HttpClient httpClient,
    AmadeusOptions options)
    : IEndpointFactory<FlightInspirationResponse, FlightInspirationFilter>
{
    private const string Path = "/v1/shopping/flight-destinations";

    public Endpoint<FlightInspirationResponse, FlightInspirationFilter> CreateEndpoint() =>
        new(TryGetFlightInspirationsAsync);

    internal async Task<Either<ErrorResponse, FlightInspirationResponse>> TryGetFlightInspirationsAsync(
        FlightInspirationFilter filter,
        CancellationToken cancellationToken)
    {
        using var request = BuildRequest(
            HttpMethod.Get,
            Path,
            filter.AsQuery());

        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<FlightInspirationResponse>(cancellationToken);
    }

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
