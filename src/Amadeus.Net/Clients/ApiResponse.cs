namespace Amadeus.Net.Clients;

public static class ApiResponse
{
    public static ApiResponse<TSuccessResponse, TErrorResponse> Success<TSuccessResponse, TErrorResponse>(TSuccessResponse data) =>
        new(data, default);

    public static ApiResponse<TSuccessResponse, TErrorResponse> Error<TSuccessResponse, TErrorResponse>(TErrorResponse error) =>
        new(default, error);

    public static ApiResponse<TSuccessResponse, TErrorResponse> Create<TSuccessResponse, TErrorResponse>(TSuccessResponse? data, TErrorResponse? error) =>
        new(data, error);

    public static ApiResponse<TSuccessResponse, TErrorResponse> Create<TSuccessResponse, TErrorResponse>((TSuccessResponse? data, TErrorResponse? error) response) =>
        new(response.data, response.error);
}

public class ApiResponse<TSuccessResponse, TErrorResponse>(
    TSuccessResponse? Data,
    TErrorResponse? Error)
{
    /// <summary>
    /// True if this response contains data
    /// </summary>
    public bool IsSuccess => Data is not null;

    /// <summary>
    /// True if this response contains error
    /// </summary>
    public bool IsError => Error is not null;

    /// <summary>
    /// Maps the data if successful, otherwise returns a default value
    /// </summary>
    /// <typeparam name="T">Target type</typeparam>
    /// <param name="map">Mapping function for successful response</param>
    /// <param name="defaultValue">Default value if error</param>
    /// <returns>Mapped value or default</returns>
    public T Map<T>(Func<TSuccessResponse, T> map, T defaultValue) =>
        Data is not null ? map(Data) : defaultValue;

    /// <summary>
    /// Maps the data if successful, otherwise returns the result of the error mapping
    /// </summary>
    /// <typeparam name="T">Target type</typeparam>
    /// <param name="successMap">Mapping function for successful response</param>
    /// <param name="errorMap">Mapping function for error response</param>
    /// <returns>Mapped value from either success or error</returns>
    public T Match<T>(Func<TSuccessResponse, T> successMap, Func<TErrorResponse, T> errorMap) =>
        Data is not null ? successMap(Data) : errorMap(Error!);

    /// <summary>
    /// Transforms the successful result of the response using the provided selector function.
    /// If the response is an error, propagates the error unchanged.
    /// </summary>
    /// <typeparam name="TResult">The type of the transformed success value.</typeparam>
    /// <param name="selector">A function to transform the success value.</param>
    /// <returns>
    /// A new <see cref="ApiResponse{TResult, TErrorResponse}"/> containing the transformed success value if present,
    /// or the original error if not.
    /// </returns>
    public ApiResponse<TResult, TErrorResponse> Select<TResult>(Func<TSuccessResponse, TResult> selector) =>
        Data is not null
            ? new ApiResponse<TResult, TErrorResponse>(selector(Data), default)
            : new ApiResponse<TResult, TErrorResponse>(default, Error);

    /// <summary>
    /// Projects the successful result of the response into a new <see cref="ApiResponse{TResult, TErrorResponse}"/>
    /// using the provided selector function. If the response is an error, propagates the error unchanged.
    /// This enables chaining of operations that may each return an <see cref="ApiResponse{TResult, TErrorResponse}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the projected success value.</typeparam>
    /// <param name="selector">A function to project the success value into a new <see cref="ApiResponse{TResult, TErrorResponse}"/>.</param>
    /// <returns>
    /// The result of the selector function if the response is successful, or the original error if not.
    /// </returns>
    public ApiResponse<TResult, TErrorResponse> SelectMany<TResult>(
        Func<TSuccessResponse, ApiResponse<TResult, TErrorResponse>> selector) =>
        Data is not null
            ? selector(Data)
            : new ApiResponse<TResult, TErrorResponse>(default, Error);
}
