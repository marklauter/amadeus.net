using Amadeus.Net.Clients.Response;
using LanguageExt;
using System.Text.Json;
using System.Text.Json.Serialization;
using static LanguageExt.Prelude;

namespace Amadeus.Net.HttpClientExtensions;

internal static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static IO<Either<ErrorResponse, TParseResult>> Parse<TParseResult>(
        this HttpResponseMessage response) =>
        response.Content.AsString().Map(content => response.IsSuccessStatusCode
            ? TryParse<TParseResult>(content)
            : TryParse<ErrorResponse>(content).Bind(e => Left<ErrorResponse, TParseResult>(e)));

    private static Either<ErrorResponse, TParseResult> TryParse<TParseResult>(string content) =>
        Try.lift(() => Parse<TParseResult>(content))
            .Match(
                Succ: r => r,
                Fail: error => Left<ErrorResponse, TParseResult>(ErrorResponse.Create(
                new ApiError(
                    -1,
                    (long)CustomErrorCode.DeserializationError,
                    $"DESERIALIZATION ERROR: Message: {error.Message}",
                    $"Could not deserialize {typeof(TParseResult).FullName} from content: {content}",
                    null))));

    private static Either<ErrorResponse, TParseResult> Parse<TParseResult>(string content) =>
        JsonSerializer.Deserialize<TParseResult>(content, JsonOptions) is { } parseResult
            ? Right<ErrorResponse, TParseResult>(parseResult)
            : Left<ErrorResponse, TParseResult>(ErrorResponse.Create(
                new ApiError(
                    -1,
                    (long)CustomErrorCode.DeserializationError,
                    "DESERIALIZATION ERROR",
                    $"Could not deserialize {typeof(TParseResult).FullName} from content: {content}",
                    null
                )
            ));

    private static IO<string> AsString(this HttpContent content) =>
        liftIO(env => content.ReadAsStringAsync(env.Token));
}
