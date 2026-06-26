using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using TikTokFeed.Engagement.IntegrationTests.Infrastructure;
using Xunit;

namespace TikTokFeed.Engagement.IntegrationTests;

public sealed class IdentityFollowTests : IClassFixture<IdentityTestFactory>
{
    private readonly IdentityTestFactory _factory;

    public IdentityFollowTests(IdentityTestFactory factory) => _factory = factory;

    [Fact]
    public async Task FollowSelf_ReturnsConflict()
    {
        var me = Guid.NewGuid();
        HttpClient client = _factory.CreateClientFor(me);

        HttpResponseMessage response = await client.PostAsync($"/api/v1/users/{me}/follow", null);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await TestJson.ErrorCode(response)).Should().Be("CANNOT_FOLLOW_SELF");
    }

    [Fact]
    public async Task FollowTwice_ReturnsConflict()
    {
        Guid target = await RegisterUserAsync();
        Guid follower = await RegisterUserAsync();
        HttpClient client = _factory.CreateClientFor(follower);

        HttpResponseMessage first = await client.PostAsync($"/api/v1/users/{target}/follow", null);
        HttpResponseMessage second = await client.PostAsync($"/api/v1/users/{target}/follow", null);

        first.StatusCode.Should().Be(HttpStatusCode.Created);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await TestJson.ErrorCode(second)).Should().Be("ALREADY_FOLLOWING");
    }

    [Fact]
    public async Task FollowUnknownUser_ReturnsNotFound()
    {
        HttpClient client = _factory.CreateClientFor(Guid.NewGuid());

        HttpResponseMessage response = await client.PostAsync($"/api/v1/users/{Guid.NewGuid()}/follow", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Guid> RegisterUserAsync()
    {
        string suffix = Guid.NewGuid().ToString("N")[..8];
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.PostAsync(
            "/api/v1/auth/register",
            TestJson.Body(new
            {
                username = "user_" + suffix,
                email = $"user_{suffix}@example.com",
                password = "Passw0rd!",
            }));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await TestJson.Root(response)).GetProperty("user_id").GetGuid();
    }
}
