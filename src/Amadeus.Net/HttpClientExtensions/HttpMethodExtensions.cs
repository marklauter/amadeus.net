using Amadeus.Net.Endpoints.Query;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

internal static class HttpMethodExtensions
{
    public static HttpRequestMessage BuildRequest(
        this HttpMethod method,
        ClientMetaData clientMetaData,
        string path,
        Seq<QueryParameter> query) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(
                name: clientMetaData.ClientName,
                version: clientMetaData.ClientVersion.ToString())
            .WithUserAgent(
                name: "dotnet",
                version: "9")
            .Accept(mediaType: "application/vnd.amadeus+json")
            .Accept(mediaType: "application/json")
            .WithQueryParameters(query)
            .Build();

    public static HttpRequestMessage BuildRequest(
        this HttpMethod method,
        ClientMetaData clientMetaData,
        string path,
        Seq<QueryParameter> query,
        HttpContent content) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(
                name: clientMetaData.ClientName,
                version: clientMetaData.ClientVersion.ToString())
            .WithUserAgent(
                name: "dotnet",
                version: "9")
            .Accept(mediaType: "application/vnd.amadeus+json")
            .Accept(mediaType: "application/json")
            .WithQueryParameters(query)
            .WithContent(content)
            .Build();

    public static HttpRequestMessage BuildRequest(
        this HttpMethod method,
        ClientMetaData clientMetaData,
        string path,
        HttpContent content) =>
        new HttpRequestMessageBuilder(method, new Uri(path, UriKind.Relative))
            .WithUserAgent(
                name: clientMetaData.ClientName,
                version: clientMetaData.ClientVersion.ToString())
            .WithUserAgent(
                name: "dotnet",
                version: "9")
            .Accept(mediaType: "application/vnd.amadeus+json")
            .Accept(mediaType: "application/json")
            .WithContent(content)
            .Build();
}
