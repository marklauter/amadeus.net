using Amadeus.Net.ApiContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Amadeus.Net.Tests;

public class AmadeusContextTests
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

    [Fact]
    public void ServiceRegistrationSucceeds()
    {
        using var services = new ServiceCollection()
            .AddAmadeusContext(configuration)
            .BuildServiceProvider();

        Assert.NotNull(services.GetRequiredService<AmadeusContext>());
    }

    // proving copilot wrong
    [Theory]
    [InlineData("https://test.api.amadeus.com/", "v1/security/oauth2/token")]
    [InlineData("https://test.api.amadeus.com", "/v1/security/oauth2/token")]
    [InlineData("https://test.api.amadeus.com/", "/v1/security/oauth2/token")]
    [InlineData("https://test.api.amadeus.com", "v1/security/oauth2/token")]
    public void UriBuildingWorksAsExpected(string host, string tokenPath)
    {
        var uri = new Uri(new Uri(host), tokenPath);
        Assert.Equal("https://test.api.amadeus.com/v1/security/oauth2/token", uri.ToString());
    }

    //[Fact]
    //public async Task FlightInspiration()
    //{
    //    using var services = new ServiceCollection()
    //        .AddAmadeusContext(configuration)
    //        .BuildServiceProvider();

    //    var context = services.GetRequiredService<AmadeusContext>();
    //    var destinations = await context
    //        .FlightInspirations
    //        .Filter(() => FlightInspirationFilter.From("PAR"))
    //        .ExecuteReaderAsync(CancellationToken.None);

    //    Assert.True(destinations.Exists(r => r.Destinations.Any()));
    //}
}
