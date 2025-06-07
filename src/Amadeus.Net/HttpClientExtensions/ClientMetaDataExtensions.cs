using Amadeus.Net.Endpoints.Query;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

internal static class ClientMetaDataExtensions
{
    public static HttpRequestMessage BuildGetRequest(
        this ClientMetaData clientMetaData,
        string path,
        Seq<QueryParameter> query) =>
            HttpMethod.Get.BuildRequest(clientMetaData, path, query);

    public static HttpRequestMessage BuildPostRequest(
        this ClientMetaData clientMetaData,
        string path,
        HttpContent content) =>
            HttpMethod.Post.BuildRequest(clientMetaData, path, content);
}
