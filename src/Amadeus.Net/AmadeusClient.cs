using Amadeus.Net.Options;
using Amadeus.Net.Requests;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amadeus.Net;

public sealed class AmadeusClient(
    HttpClient httpClient,
    AmadeusOptions options,
    ILogger<AmadeusClient> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private const string FlightInpirationPath = "/v1/shopping/flight-destinations";
    public async Task<string> ReadFlightInspirationAsync(
        string origin,
        CancellationToken cancellationToken)
    {
        using var request = BuildRequest(
            HttpMethod.Get,
            FlightInpirationPath,
            [
                KeyValuePair.Create("origin", origin),
                KeyValuePair.Create("maxPrice", "200")]);

        return await SendAsync(request, cancellationToken);
    }

    private async Task<string> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return !response.IsSuccessStatusCode
            ? throw new InvalidOperationException($"request failed for '{request.RequestUri}', code: {response.StatusCode}, reason: {response.ReasonPhrase}, content: {content}")
            : content;
    }

    private async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await httpClient.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return !response.IsSuccessStatusCode
            ? throw new InvalidOperationException($"failed getting token from '{request.RequestUri}', code: {response.StatusCode}, reason: {response.ReasonPhrase}, content: {content}")
            : JsonSerializer.Deserialize<T>(content, JsonOptions)
                ?? throw new InvalidOperationException($"could not deserialize token response from '{request.RequestUri}', content: {content}");
    }

    private HttpRequestMessage BuildRequest(HttpMethod method, string path)
        => BuildRequest(method, path, null);

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        IEnumerable<KeyValuePair<string, string>>? query)
    {
        var builder = new HttpRequestMessageBuilder(method, path)
            .WithUserAgent(options.ClientName, options.ClientVersion.ToString())
            .WithUserAgent("dotnet", "9")
            .Accept("application/vnd.amadeus+json")
            .Accept("application/json");
        return query is not null
            ? builder.WithQueryParameters(query).Build()
            : builder.Build();
    }
}
