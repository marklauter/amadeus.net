using Amadeus.Net.Context;
using Amadeus.Net.Endpoints.AirportCitySearch;
using Amadeus.Net.HttpClientExtensions;
using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Amadeus.Net.Tests.Clients;

public sealed class AirportCitySearchClientTests
{
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "Amadeus:Host", "https://test.api.amadeus.com" },
            { "Amadeus:ClientMetaData:ClientVersion", "0.0.0" },
            { "Amadeus:ClientMetaData:ClientName", "TWAI" },
        })
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true, false)
        .Build();

    [Fact]
    public async Task ContextTest()
    {
        var tkn = CancellationToken.None;
        var response =
            await Prelude.use(
                acquire: () => new ServiceCollection()
                    .AddAmadeusContext(configuration)
                    .BuildServiceProvider(),
                release: provider => provider.Dispose())
            .Map(provider => provider.GetRequiredService<AmadeusContext>())
            .Bind(context => context.AirportCities.Get(
                AirportCityQuery
                .StartsWith("MUC")
                .IncludeAirports()
                .IncludeCities()))
            .InvokeAsync(tkn);

        _ = response.Match(
            Left: error => Assert.Fail($"expected success: {error}"),
            Right: r =>
                {
                    Assert.Equal(2, r.Locations.Count);
                    Assert.NotEmpty(r.Locations.Select(l => l.Name.Equals("Munich", StringComparison.OrdinalIgnoreCase)));
                });
    }
}
