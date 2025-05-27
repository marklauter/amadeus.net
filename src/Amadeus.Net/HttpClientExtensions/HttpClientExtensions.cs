using Amadeus.Net.Clients;
using Amadeus.Net.Clients.Response;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

public static class HttpClientExtensions
{
    public static IO<HttpResponseMessage> SendIO(this HttpClient httpClient, HttpRequestMessage request) =>
        Prelude.liftIO(e => httpClient.SendAsync(request, e.Token));

    public static IO<Either<ErrorResponse, T>> GetIO<T>(this HttpClient httpClient, HttpRequestMessage request) =>
        Prelude.use(
            acquire: httpClient.SendIO(request),
            release: response => response.Dispose())
            .Bind(response => response.Parse<T>());

    public static IO<Either<ErrorResponse, R>> Filter<F, R>(
        this HttpClient httpClient,
        AmadeusOptions options,
        string path,
        F filter) where F : IFilter =>
        Prelude.use(
            acquire: () => options.BuildGetRequest(path, filter.AsQuery()),
            release: request => request.Dispose())
            .Bind(httpClient.GetIO<R>);

}
