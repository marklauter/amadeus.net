using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

public static class HttpClientExtensions
{
    public static IO<HttpResponseMessage> SendIO(this HttpClient httpClient, HttpRequestMessage request) =>
        Prelude.liftIO(e => httpClient.SendAsync(request, e.Token));
}
