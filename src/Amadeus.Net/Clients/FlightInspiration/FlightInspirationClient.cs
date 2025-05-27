using Amadeus.Net.Clients.FlightInspiration.Response;
using Amadeus.Net.Clients.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.Clients.FlightInspiration;

internal sealed class FlightInspirationClient(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private const string Path = "/v1/shopping/flight-destinations";

    internal IO<Either<ErrorResponse, FlightInspirationResponse>> Filter(
        FlightInspirationFilter filter) =>
            Prelude.use(
                acquire: () => BuildRequest(HttpMethod.Get, Path, filter.AsQuery()),
                release: request => request.Dispose())
                .Bind(httpClient.GetIO<FlightInspirationResponse>);

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
