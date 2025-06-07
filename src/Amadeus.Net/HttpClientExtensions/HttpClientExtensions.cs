using Amadeus.Net.Endpoints.Query;
using Amadeus.Net.Endpoints.Response;
using Amadeus.Net.Options;
using LanguageExt;

namespace Amadeus.Net.HttpClientExtensions;

public static class HttpClientExtensions
{
    public static IO<Either<ErrorResponse, R>> Post<R>(
        this HttpClient httpClient,
        ClientMetaData clientMetaData,
        string path,
        HttpContent content) =>
        Prelude.use(
            acquire: () => clientMetaData.BuildPostRequest(path, content),
            release: request => request.Dispose())
            .Bind(httpClient.SendIO<R>);

    public static IO<Either<ErrorResponse, R>> Get<Q, R>(
        this HttpClient httpClient,
        ClientMetaData clientMetaData,
        string path,
        Q query)
        where Q : IQuery =>
        Prelude.use(
            acquire: () => clientMetaData.BuildGetRequest(path, query.ToParams()),
            release: request => request.Dispose())
            .Bind(httpClient.SendIO<R>);

    private static IO<Either<ErrorResponse, R>> SendIO<R>(
        this HttpClient httpClient,
        HttpRequestMessage request) =>
        Prelude.use(
            acquire: Prelude.liftIO(env => httpClient.SendAsync(request, env.Token)),
            release: response => response.Dispose())
            .Bind(response => response.Parse<R>());

}
