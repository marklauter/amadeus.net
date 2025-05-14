using Amadeus.Net.Clients.FlightInspiration.Models;
using Amadeus.Net.Clients.LINQ;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using LanguageExt;

namespace Amadeus.Net.Clients.FlightInspiration;

internal sealed class FlightInspirationClient(
    HttpClient httpClient,
    AmadeusOptions options)
    : IEndpointFactory<FlightDestinations, FlightInspirationFilter>
{
    private const string Path = "/v1/shopping/flight-destinations";

    public Endpoint<FlightDestinations, FlightInspirationFilter> CreateEndpoint() =>
        new(TryGetFlightInspirationsAsync);

    internal async Task<Either<ErrorResponse, FlightDestinations>> TryGetFlightInspirationsAsync(
        Option<FlightInspirationFilter> filter,
        CancellationToken cancellationToken)
    {
        var queryParams = filter.Match(
            Some: (f) => f.AsQueryParams().ToArray(),
            None: () => []
        );

        using var request = BuildRequest(
            HttpMethod.Get,
            Path,
            queryParams);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<FlightDestinations>(cancellationToken);
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
