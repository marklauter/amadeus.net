using Amadeus.Net.Clients.Models;
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

    public static async Task<Either<ErrorResponse, TParseResult>> TryParseAsync<TParseResult>(
        this HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return response.IsSuccessStatusCode
            ? JsonSerializer.Deserialize<TParseResult>(content, JsonOptions) is { } parseResult
                ? parseResult
                : ErrorResponse.Create(
                    new ApiError(
                        -1,
                        (long)CustomErrorCode.DeserializationError,
                        "DATA DESERIALIZATION ERROR",
                        $"Could not deserialize {typeof(TParseResult).FullName} from response: {content}",
                        null
                    )
                )
            : JsonSerializer.Deserialize<ErrorResponse>(content, JsonOptions)
                ?? ErrorResponse.Create(
                    new ApiError(
                        -1,
                        (long)CustomErrorCode.ErrorResponseDeserializationError,
                        "ERROR RESPONSE DESERIALIZATION ERROR",
                        $"Could not deserialize ErrorResponse from response: {content}",
                        null
                    )
                );
    }

    public static IO<Either<ErrorResponse, TParseResult>> Parse<TParseResult>(
        this HttpResponseMessage response) =>
        response.Content
        .ReadAsString()
        .Map(contentString =>
            response.IsSuccessStatusCode
                ? JsonSerializer.Deserialize<TParseResult>(contentString, JsonOptions) is { } parseResult
                    ? Right<ErrorResponse, TParseResult>(parseResult)
                    : Left<ErrorResponse, TParseResult>(ErrorResponse.Create(
                        new ApiError(
                            -1,
                            (long)CustomErrorCode.DeserializationError,
                            "DATA DESERIALIZATION ERROR",
                            $"Could not deserialize {typeof(TParseResult).FullName} from response: {contentString}",
                            null
                        )
                    ))
                : JsonSerializer.Deserialize<ErrorResponse>(contentString, JsonOptions) is { } errorResponse
                    ? Left<ErrorResponse, TParseResult>(errorResponse)
                    : Left<ErrorResponse, TParseResult>(ErrorResponse.Create(
                        new ApiError(
                            -1,
                            (long)CustomErrorCode.ErrorResponseDeserializationError,
                            "ERROR RESPONSE DESERIALIZATION ERROR",
                            $"Could not deserialize ErrorResponse from response: {contentString}",
                            null
                        ))));

    private static IO<string> ReadAsString(this HttpContent content) =>
        liftIO(env => content.ReadAsStringAsync(env.Token));
}
