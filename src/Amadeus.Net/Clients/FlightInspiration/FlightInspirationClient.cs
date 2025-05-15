using Amadeus.Net.ApiContext;
using Amadeus.Net.Clients.FlightInspiration.Models;
using Amadeus.Net.Clients.Models;
using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using Amadeus.Net.Responses;
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
        FlightInspirationFilter filter,
        CancellationToken cancellationToken)
    {
        using var request = BuildRequest(
            HttpMethod.Get,
            Path,
            filter.AsQueryParams());

        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await response.TryParseAsync<FlightDestinations>(cancellationToken);
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        IEnumerable<KeyValuePair<string, string>> query) =>
        new HttpRequestMessageBuilder(method, new Uri(path))
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
