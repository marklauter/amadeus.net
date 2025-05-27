using Amadeus.Net.HttpClientExtensions;
using LanguageExt;

namespace Amadeus.Net.Clients.AirportCitySearch;

public static class HttpRequestMessageBuilderExtensions
{
    public static HttpRequestMessageBuilder WithQueryParameters(
        this HttpRequestMessageBuilder builder,
        Seq<KeyValuePair<string, string>> query) =>
        query.Match(
            Empty: () => builder,
            Seq: builder.WithQueryParameters
        );
}

