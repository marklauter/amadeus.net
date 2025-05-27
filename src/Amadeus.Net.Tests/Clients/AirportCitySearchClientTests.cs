using Amadeus.Net.Auth;
using Amadeus.Net.Clients.AirportCitySearch;
using Amadeus.Net.Options;
using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Amadeus.Net.Tests.Clients;

public sealed class AirportCitySearchClientTests
{
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Amadeus:Host", "https://test.api.amadeus.com" },
            { "Amadeus:ClientVersion", "0.0.0" },
            { "Amadeus:ClientName", "TWAI" },
        }!)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true, false)
        .Build();

    private ServiceProvider CreateServiceProvider()
    {
        var options = configuration
            .GetRequiredSection(AmadeusOptions.SectionName)
            .Get<AmadeusOptions>()
            ?? throw new InvalidOperationException($"can't read {nameof(AmadeusOptions)} in section {AmadeusOptions.SectionName}");

        var credentials = configuration
            .GetRequiredSection(nameof(AmadeusCredentials))
            .Get<AmadeusCredentials>()
            ?? throw new InvalidOperationException($"can't read {nameof(AmadeusCredentials)} in section {nameof(AmadeusCredentials)}");

        var services = new ServiceCollection()
            .AddSingleton(options)
            .AddSingleton(credentials)
            .AddTransient<AuthTokenHandler>();

        _ = services
            .AddHttpClient<TokenProvider>(client => client.BaseAddress = options.Host);

        _ = services
            .AddHttpClient<AirportCitySearchClient>(client => client.BaseAddress = options.Host)
            .AddHttpMessageHandler<AuthTokenHandler>();

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task FilterTest()
    {
        var tkn = CancellationToken.None;
        var response =
            await Prelude.use(
                acquire: CreateServiceProvider,
                release: provider => provider.Dispose())
            .Map(provider => provider.GetRequiredService<AirportCitySearchClient>())
            .Bind(client => client.Filter(AirportCitySearchFilter
                .From("MUC")
                .WithAirports()
                .WithCities()))
            .Fork()
            .Await()
            .RunAsync(EnvIO.New(token: tkn));

        Assert.True(response.IsRight);
        _ = response.Match(
            Left: error => Assert.Fail("error"),
            Right: e => _ = e.Match(
                Left: x =>
                {
                    Assert.Equal(2, x.Data.Count);
                    Assert.NotEmpty(x.Data.Select(l => l.Name.Equals("Munich", StringComparison.OrdinalIgnoreCase)));
                },
                Right: y => Assert.Fail("expected city search response")));
    }
}
