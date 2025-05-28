using Amadeus.Net.Clients;
using Amadeus.Net.Clients.Response;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

public static class HttpClientExtensions
{
    public static IO<Either<ErrorResponse, R>> Get<Q, R>(this HttpClient httpClient, AmadeusOptions options, string path, Q query)
        where Q : IQuery =>
        Prelude.use(
            acquire: () => options.BuildGetRequest(path, query.ToParams()),
            release: request => request.Dispose())
            .Bind(httpClient.GetIO<R>);

    private static IO<Either<ErrorResponse, T>> GetIO<T>(this HttpClient httpClient, HttpRequestMessage request) =>
        Prelude.use(
            acquire: httpClient.SendIO(request),
            release: response => response.Dispose())
            .Bind(response => response.Parse<T>());

    private static IO<HttpResponseMessage> SendIO(this HttpClient httpClient, HttpRequestMessage request) =>
        Prelude.liftIO(env => httpClient.SendAsync(request, env.Token));
}
