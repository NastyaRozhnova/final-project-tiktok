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
using TikTokFeed.Engagement.Api;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.IntegrationTests.Fakes;
using TikTokFeed.Engagement.Infrastructure.Persistence;

namespace TikTokFeed.Engagement.IntegrationTests.Infrastructure;

public sealed class EngagementTestFactory : WebApplicationFactory<ApiMarker>
{
    private readonly string _dbName = "engagement-tests-" + Guid.NewGuid();

    public FakeContentGateway Content { get; } = new();

    public FakeIdentityGateway Identity { get; } = new();

    public HttpClient CreateClientFor(Guid userId, string username = "tester")
    {
        HttpClient client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TestTokens.Issue(userId, username));
        return client;
    }

    public EngagementDbContext CreateDbContext()
    {
        IServiceScope scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<EngagementDbContext>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(TestConstants.JwtConfig));

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<EngagementDbContext>>();
            services.AddDbContext<EngagementDbContext>(options => options.UseInMemoryDatabase(_dbName));

            services.RemoveAll<IContentGateway>();
            services.RemoveAll<IIdentityGateway>();
            services.AddSingleton<IContentGateway>(Content);
            services.AddSingleton<IIdentityGateway>(Identity);
        });
    }
}
