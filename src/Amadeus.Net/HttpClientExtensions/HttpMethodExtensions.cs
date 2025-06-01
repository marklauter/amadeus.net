using Amadeus.Net.Clients;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

internal static class HttpMethodExtensions
{
    public static HttpRequestMessage BuildGetRequest(
        this AmadeusOptions options,
        string path,
        Seq<QueryParameter> query) =>
        HttpMethod.Get.BuildRequest(options, path, query);

    public static HttpRequestMessage BuildRequest(
        this HttpMethod method,
        AmadeusOptions options,
        string path,
        Seq<QueryParameter> query) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
