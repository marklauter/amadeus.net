using Amadeus.Net.Clients.AirlineCodeLookup.Models;
using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amadeus.Net.Clients.AirlineCodeLookup;

/// <summary>
/// Airline Code Lookup API version 1.2.1
/// </summary>
/// <param name="httpClient"></param>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <remarks>
/// http://www.iata.org/publications/Pages/code-search.aspx
/// </remarks>
public sealed class AirlineCodeLookupClient(
    HttpClient httpClient,
    AmadeusOptions options)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private const string AirlineCodeLookupPath = "/v1/reference-data/airlines";

    public Task<AirlineCodeLookupResponse> GetAirlinesByCodeAsync(
        string airlineCode,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(airlineCode);

        return GetAirlinesByCodesAsync([airlineCode], cancellationToken);
    }

    public async Task<AirlineCodeLookupResponse> GetAirlinesByCodesAsync(
        IEnumerable<string> airlineCodes,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(airlineCodes);
        if (!airlineCodes.Any())
            throw new ArgumentException("supply at least one airline code", nameof(airlineCodes));

        using var request = BuildRequest(
            HttpMethod.Get,
            AirlineCodeLookupPath,
            [KeyValuePair.Create("airlineCodes", string.Join(',', airlineCodes))]);

        return AirlineCodeLookupResponse.Create(await SendAsync<Airlines, ErrorResponse>(request, cancellationToken));
    }

    public async Task<AirlineCodeLookupResponse> GetAirlinesAsync(CancellationToken cancellationToken)
    {
        using var request = BuildRequest(HttpMethod.Get, AirlineCodeLookupPath);
        return AirlineCodeLookupResponse.Create(await SendAsync<Airlines, ErrorResponse>(request, cancellationToken));
    }

    private async Task<(TSuccessResponse? success, TErrorResponse? error)> SendAsync<TSuccessResponse, TErrorResponse>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return response.IsSuccessStatusCode
            ? (JsonSerializer.Deserialize<TSuccessResponse>(content, JsonOptions)
                ?? throw new InvalidOperationException($"Could not deserialize success response from '{request.RequestUri}', content: {content}"), default)
            : (default, JsonSerializer.Deserialize<TErrorResponse>(content, JsonOptions)
                ?? throw new InvalidOperationException($"Could not deserialize error response from '{request.RequestUri}', content: {content}"));
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path) =>
        new HttpRequestMessageBuilder(method, path)
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .Build();

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        IEnumerable<KeyValuePair<string, string>> query) =>
        new HttpRequestMessageBuilder(method, path)
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json")
            .WithQueryParameters(query)
            .Build();
}
