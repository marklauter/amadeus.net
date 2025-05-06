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
}
