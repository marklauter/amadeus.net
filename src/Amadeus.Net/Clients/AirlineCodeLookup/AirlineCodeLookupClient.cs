using Amadeus.Net.Clients.AirlineCodeLookup.Response;
using Amadeus.Net.Clients.Response;
using Amadeus.Net.HttpClientExtensions;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

internal sealed class AirlineCodeLookupClient(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private const string Path = "/v1/reference-data/airlines";

    internal IO<Either<ErrorResponse, AirlineLookupResponse>> Filter(
        AirlineCodeFilter filter) =>
        Prelude.use(
            acquire: () => BuildRequest(HttpMethod.Get, Path, filter.AsQuery()),
            release: request => request.Dispose())
            .Bind(httpClient.GetIO<AirlineLookupResponse>);

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
