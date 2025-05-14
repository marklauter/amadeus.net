using Amadeus.Net.ApiContext;
using Amadeus.Net.Auth;
using Amadeus.Net.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amadeus.Net;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmadeusContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetRequiredSection(AmadeusOptions.SectionName)
            .Get<AmadeusOptions>()
            ?? throw new InvalidOperationException($"can't read {nameof(AmadeusOptions)} in section {AmadeusOptions.SectionName}");

        var credentials = configuration
            .GetRequiredSection(nameof(AmadeusCredentials))
            .Get<AmadeusCredentials>()
            ?? throw new InvalidOperationException($"can't read {nameof(AmadeusCredentials)} in section {nameof(AmadeusCredentials)}");

        _ = services
            .AddSingleton(options)
            .AddSingleton(credentials)
            .AddTransient<AuthTokenHandler>();

        _ = services
            .AddHttpClient<TokenProvider>(client => client.BaseAddress = options.Host);

        _ = services
            .AddHttpClient<AmadeusContext>(client => client.BaseAddress = options.Host)
            .AddHttpMessageHandler<AuthTokenHandler>();

        return services;
    }
}
