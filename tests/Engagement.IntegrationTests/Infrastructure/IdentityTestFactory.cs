using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TikTokFeed.Identity.Api;
using TikTokFeed.Identity.Infrastructure.Persistence;

namespace TikTokFeed.Engagement.IntegrationTests.Infrastructure;

public sealed class IdentityTestFactory : WebApplicationFactory<TikTokFeed.Identity.Api.ApiMarker>
{
    private readonly string _dbName = "identity-tests-" + Guid.NewGuid();

    public HttpClient CreateClientFor(Guid userId, string username = "tester")
    {
        HttpClient client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TestTokens.Issue(userId, username));
        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(TestConstants.JwtConfig));

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<IdentityDbContext>>();
            services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase(_dbName));
        });
    }
}
