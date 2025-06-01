using Amadeus.Net.Clients;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

internal static class HttpMethodExtensions
{
    public static HttpRequestMessage BuildGetRequest(
        this ClientMetaData clientMetaData,
        string path,
        Seq<QueryParameter> query) =>
            HttpMethod.Get.BuildRequest(clientMetaData, path, query);

    public static HttpRequestMessage BuildPostRequest(
        this ClientMetaData clientMetaData,
        string path,
        Seq<QueryParameter> query) =>
            HttpMethod.Post.BuildRequest(clientMetaData, path, query);

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
}
