using LanguageExt;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Amadeus.Net.Requests;

[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "HttpRequestMessage is returned to the caller which is responsible for disposal.")]
public sealed class HttpRequestMessageBuilder(
    HttpMethod method,
    Option<Uri> uri)
{
    public enum CacheType
    {
        NoCache,
        NoStore,
        MaxAge,
        MinFresh,
        Public,
        Private,
        MustRevalidate,
        ProxyRevalidate,
        OnlyIfCached
    }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable", Justification = "it's not ignored")]
    private readonly HttpRequestMessage request = uri.Match(
        Some: uri => new HttpRequestMessage(method ?? throw new ArgumentNullException(nameof(method)), uri),
        None: new HttpRequestMessage(method ?? throw new ArgumentNullException(nameof(method)), (string?)null));

    private readonly List<KeyValuePair<string, string>> queryParameters = [];

    public HttpRequestMessageBuilder()
        : this(HttpMethod.Get, Option<Uri>.None)
    {
    }

    public HttpRequestMessageBuilder(Uri uri)
        : this(HttpMethod.Get, uri ?? throw new ArgumentNullException(nameof(uri)))
    {
    }

    public HttpRequestMessageBuilder WithMethod(HttpMethod method)
    {
        request.Method = method ?? throw new ArgumentNullException(nameof(method));
        return this;
    }

    public HttpRequestMessageBuilder WithUri(Uri uri)
    {
        request.RequestUri = uri ?? throw new ArgumentNullException(nameof(uri));
        return this;
    }

    public HttpRequestMessageBuilder WithQueryParameter(string key, string value) =>
        WithQueryParameter(KeyValuePair.Create(key, value));

    public HttpRequestMessageBuilder WithQueryParameter(KeyValuePair<string, string> parameter)
    {
        queryParameters.Add(parameter);
        return this;
    }

    public HttpRequestMessageBuilder WithQueryParameters(params KeyValuePair<string, string>[] parameters)
    {
        if (parameters.Length > 0)
            queryParameters.AddRange(parameters);

        return this;
    }

    public HttpRequestMessageBuilder WithHeader(string key, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        ArgumentException.ThrowIfNullOrEmpty(value);

        request.Headers.Add(key, value);
        return this;
    }

    public HttpRequestMessageBuilder WithHeaders(params KeyValuePair<string, string>[] headers)
    {
        if (headers.Length == 0)
            return this;

        foreach (var (key, value) in headers)
        {
            ArgumentException.ThrowIfNullOrEmpty(key);
            ArgumentException.ThrowIfNullOrEmpty(value);

            request.Headers.Add(key, value);
        }

        return this;
    }

    public HttpRequestMessageBuilder WithAuthorizationHeader(string scheme, string parameter)
    {
        ArgumentException.ThrowIfNullOrEmpty(scheme, nameof(scheme));
        ArgumentException.ThrowIfNullOrEmpty(parameter, nameof(parameter));

        request.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
        return this;
    }

    public HttpRequestMessageBuilder WithContent(HttpContent content)
    {
        request.Content = request.Content is not null
            ? throw new InvalidOperationException("The request content has already been set. Only one content body is allowed per request.")
            : content ?? throw new ArgumentNullException(nameof(content));

        return this;
    }

    public HttpRequestMessageBuilder WithFormContent(params KeyValuePair<string, string>[] formData) =>
        formData.Length == 0
        ? this
        : WithContent(new FormUrlEncodedContent(formData));

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created", Justification = "multipartContent is added to request")]
    public HttpRequestMessageBuilder WithMultipartContent(params HttpContent[] contents)
    {
        if (contents.Length == 0)
            return this;

        var multipartContent = new MultipartFormDataContent();
        foreach (var content in contents)
        {
            ArgumentNullException.ThrowIfNull(content);
            multipartContent.Add(content);
        }

        return WithContent(multipartContent);
    }

    public HttpRequestMessageBuilder WithStringContent(
        string content) =>
        WithStringContent(content, Encoding.UTF8);

    public HttpRequestMessageBuilder WithStringContent(
        string content,
        Encoding encoding) =>
        WithStringContent(content, encoding, "text/plain");

    public HttpRequestMessageBuilder WithStringContent(
        string content,
        Encoding encoding,
        string mediaType)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(encoding);
        ArgumentException.ThrowIfNullOrWhiteSpace(mediaType);

        return WithContent(new StringContent(content, encoding, mediaType));
    }

    public HttpRequestMessageBuilder WithJsonContent<T>(
        T content)
    {
        ArgumentNullException.ThrowIfNull(content);

        return WithContent(JsonContent.Create(content));
    }

    public HttpRequestMessageBuilder WithJsonContent<T>(
        T content,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(options);

        return WithContent(JsonContent.Create(content, options: options));
    }

    public HttpRequestMessageBuilder WithCookie(string name, string value)
    {
        var cookies = CreateCookie(ValidateCookie(KeyValuePair.Create(name, value)));
        if (request.Headers.TryGetValues("Cookie", out var existingCookies))
        {
            cookies = $"{existingCookies.First()}; {cookies}";
            _ = request.Headers.Remove("Cookie");
        }

        request.Headers.Add("Cookie", cookies);
        return this;
    }

    public HttpRequestMessageBuilder WithCookies(IEnumerable<KeyValuePair<string, string>> cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies);

        var newCookies = string.Join("; ", cookies
            .Select(ValidateCookie)
            .Select(CreateCookie));

        if (request.Headers.TryGetValues("Cookie", out var existingCookies))
        {
            newCookies = $"{existingCookies.First()}; {newCookies}";
            _ = request.Headers.Remove("Cookie");
        }

        request.Headers.Add("Cookie", newCookies);
        return this;
    }

    private static string CreateCookie(KeyValuePair<string, string> cookie) =>
        $"{Uri.EscapeDataString(cookie.Key)}={Uri.EscapeDataString(cookie.Value)}";

    private static KeyValuePair<string, string> ValidateCookie(KeyValuePair<string, string> cookie)
    {
        ArgumentException.ThrowIfNullOrEmpty(cookie.Key);
        ArgumentException.ThrowIfNullOrEmpty(cookie.Value);

        return cookie.Key.Contains(';') || cookie.Value.Contains(';')
            ? throw new ArgumentException($"Cookie names and values cannot contain semicolons. Key: '{cookie.Key}', Value: '{cookie.Value}'")
            : cookie;
    }

    public HttpRequestMessageBuilder WithUserAgent(string name, string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(version);
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue(name, version));
        return this;
    }

    public HttpRequestMessageBuilder WithOrigin(string origin)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(origin);

        request.Headers.Add("Origin", origin);
        return this;
    }

    public HttpRequestMessageBuilder WithCacheControl(string cacheControl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cacheControl);

        request.Headers.CacheControl = CacheControlHeaderValue.Parse(cacheControl);
        return this;
    }

    public HttpRequestMessageBuilder WithCacheControl(CacheType cacheType, TimeSpan? duration = null)
    {
        request.Headers.CacheControl ??= new CacheControlHeaderValue();
        switch (cacheType)
        {
            case CacheType.NoCache:
                request.Headers.CacheControl.NoCache = true;
                break;
            case CacheType.NoStore:
                request.Headers.CacheControl.NoStore = true;
                break;
            case CacheType.MaxAge:
                request.Headers.CacheControl.MaxAge = duration ?? throw new ArgumentNullException(nameof(duration));
                break;
            case CacheType.MinFresh:
                request.Headers.CacheControl.MinFresh = duration ?? throw new ArgumentNullException(nameof(duration));
                break;
            case CacheType.Public:
                request.Headers.CacheControl.Public = true;
                break;
            case CacheType.Private:
                request.Headers.CacheControl.Private = true;
                break;
            case CacheType.MustRevalidate:
                request.Headers.CacheControl.MustRevalidate = true;
                break;
            case CacheType.ProxyRevalidate:
                request.Headers.CacheControl.ProxyRevalidate = true;
                break;
            case CacheType.OnlyIfCached:
                request.Headers.CacheControl.OnlyIfCached = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cacheType), cacheType, "Unsupported cache type.");
        }

        return this;
    }

    public HttpRequestMessageBuilder WithReferer(string referer)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(referer);

        request.Headers.Referrer = new Uri(referer);
        return this;
    }

    public HttpRequestMessageBuilder Accept(string mediaType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mediaType);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        return this;
    }

    public HttpRequestMessageBuilder AcceptLanguage(string language)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(language);

        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
        return this;
    }

    public HttpRequestMessageBuilder AcceptEncoding(string encoding)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encoding);

        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(encoding));
        return this;
    }

    public HttpRequestMessage Build()
    {
        if (queryParameters is { Count: > 0 })
        {
            request.RequestUri = request.RequestUri is not null
                ? new Uri(
                    $"{request.RequestUri}?{string.Join('&', queryParameters.Select(item => $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value)}"))}",
                    UriKind.Relative)
                : new Uri(
                    $"?{string.Join('&', queryParameters.Select(item => $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value)}"))}",
                    UriKind.Relative);
        }

        return request;
    }
}
