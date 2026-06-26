using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.IntegrationTests.Infrastructure;
using TikTokFeed.Engagement.Infrastructure.Persistence;
using Xunit;

namespace TikTokFeed.Engagement.IntegrationTests;

public sealed class EngagementCornerCaseTests : IClassFixture<EngagementTestFactory>
{
    private readonly EngagementTestFactory _factory;

    public EngagementCornerCaseTests(EngagementTestFactory factory) => _factory = factory;

    [Fact]
    public async Task Like_PendingVideo_ReturnsForbidden()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.Add(owner: Guid.NewGuid(), moderation: "PENDING");
        HttpClient client = _factory.CreateClientFor(user);

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await TestJson.ErrorCode(response)).Should().Be("VIDEO_NOT_APPROVED");
    }

    [Fact]
    public async Task Like_RejectedVideo_ReturnsForbidden()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.Add(owner: Guid.NewGuid(), moderation: "REJECTED");
        HttpClient client = _factory.CreateClientFor(user);

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Like_Duplicate_ReturnsConflict()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.AddApproved(owner: Guid.NewGuid());
        HttpClient client = _factory.CreateClientFor(user);

        HttpResponseMessage first = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);
        HttpResponseMessage second = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);

        first.StatusCode.Should().Be(HttpStatusCode.Created);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await TestJson.ErrorCode(second)).Should().Be("LIKE_ALREADY_EXISTS");
    }

    [Fact]
    public async Task Like_MissingVideo_ReturnsNotFound()
    {
        HttpClient client = _factory.CreateClientFor(Guid.NewGuid());

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{Guid.NewGuid()}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PrivateVideo_NonFollower_ReturnsForbidden()
    {
        var owner = Guid.NewGuid();
        var stranger = Guid.NewGuid();
        VideoView video = _factory.Content.Add(owner, moderation: "APPROVED", isPublic: false);
        HttpClient client = _factory.CreateClientFor(stranger);

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PrivateVideo_Follower_CanLike()
    {
        var owner = Guid.NewGuid();
        var follower = Guid.NewGuid();
        _factory.Identity.Following[follower] = new System.Collections.Generic.List<Guid> { owner };
        VideoView video = _factory.Content.Add(owner, moderation: "APPROVED", isPublic: false);
        HttpClient client = _factory.CreateClientFor(follower);

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{video.VideoId}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Comment_MissingParent_ReturnsNotFound()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.AddApproved(owner: Guid.NewGuid());
        HttpClient client = _factory.CreateClientFor(user);

        HttpResponseMessage response = await client.PostAsync(
            $"/api/v1/videos/{video.VideoId}/comments",
            TestJson.Body(new { comment_text = "reply", parent_comment_id = Guid.NewGuid() }));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await TestJson.ErrorCode(response)).Should().Be("PARENT_COMMENT_NOT_FOUND");
    }

    [Fact]
    public async Task Comment_ValidParent_Succeeds()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.AddApproved(owner: Guid.NewGuid());
        HttpClient client = _factory.CreateClientFor(user);

        HttpResponseMessage parentResponse = await client.PostAsync(
            $"/api/v1/videos/{video.VideoId}/comments",
            TestJson.Body(new { comment_text = "parent" }));
        parentResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var parentId = (await TestJson.Root(parentResponse)).GetProperty("comment_id").GetGuid();

        HttpResponseMessage replyResponse = await client.PostAsync(
            $"/api/v1/videos/{video.VideoId}/comments",
            TestJson.Body(new { comment_text = "child", parent_comment_id = parentId }));

        replyResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task View_RepeatedIdempotencyKey_RecordedOnce()
    {
        var user = Guid.NewGuid();
        VideoView video = _factory.Content.AddApproved(owner: Guid.NewGuid());
        HttpClient client = _factory.CreateClientFor(user);
        var key = Guid.NewGuid().ToString();

        for (int i = 0; i < 3; i++)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/videos/{video.VideoId}/views")
            {
                Content = TestJson.Body(new { watch_duration = 12 }),
            };
            request.Headers.Add("Idempotency-Key", key);
            HttpResponseMessage response = await client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        using IServiceScope scope = _factory.Services.CreateScope();
        EngagementDbContext db = scope.ServiceProvider.GetRequiredService<EngagementDbContext>();
        int count = await db.Views.CountAsync(v => v.UserId == user && v.VideoId == video.VideoId);
        count.Should().Be(1);
    }

    [Fact]
    public async Task MissingToken_ReturnsUnauthorized()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsync($"/api/v1/videos/{Guid.NewGuid()}/likes", null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await TestJson.ErrorCode(response)).Should().Be("UNAUTHORIZED");
    }

    [Fact]
    public async Task EmptyFeed_ReturnsNullCursor()
    {
        HttpClient client = _factory.CreateClientFor(Guid.NewGuid());

        HttpResponseMessage response = await client.GetAsync("/api/v1/feed");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonElement root = await TestJson.Root(response);
        root.GetProperty("videos").GetArrayLength().Should().Be(0);
        TestJson.IsNullOrMissing(root, "next_cursor").Should().BeTrue();
    }

    [Fact]
    public async Task Feed_LastPage_HasNullCursor()
    {
        var viewer = Guid.NewGuid();
        var author = Guid.NewGuid();
        _factory.Identity.Following[viewer] = new System.Collections.Generic.List<Guid> { author };
        for (int i = 0; i < 3; i++)
        {
            _factory.Content.AddApproved(owner: author);
        }

        HttpClient client = _factory.CreateClientFor(viewer);

        JsonElement firstPage = await TestJson.Root(await client.GetAsync("/api/v1/feed?limit=2"));
        firstPage.GetProperty("videos").GetArrayLength().Should().Be(2);
        var cursor = firstPage.GetProperty("next_cursor").GetString();
        cursor.Should().NotBeNull();

        JsonElement lastPage = await TestJson.Root(await client.GetAsync($"/api/v1/feed?limit=2&cursor={cursor}"));
        TestJson.IsNullOrMissing(lastPage, "next_cursor").Should().BeTrue();
    }
}
