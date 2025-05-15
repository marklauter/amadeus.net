using Amadeus.Net.Requests;
using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

public static class HttpRequestMessageBuilderExtensions
{
    public static HttpRequestMessageBuilder WithQueryParameters(
        this HttpRequestMessageBuilder builder,
        Option<IEnumerable<KeyValuePair<string, string>>> query) =>
        query.Match(
            Some: builder.WithQueryParameters,
            None: () => builder
        );
}

