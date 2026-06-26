using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TikTokFeed.Engagement.Application.Abstractions.Services;

namespace TikTokFeed.Engagement.IntegrationTests.Fakes;

public sealed class FakeContentGateway : IContentGateway
{
    public Dictionary<Guid, VideoView> Videos { get; } = new();

    public Task<VideoView?> GetVideoAsync(Guid videoId, CancellationToken cancellationToken) =>
        Task.FromResult(Videos.TryGetValue(videoId, out VideoView? video) ? video : null);

    public Task<IReadOnlyList<VideoView>> GetVideosByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        Task.FromResult((IReadOnlyList<VideoView>)Videos.Values.Where(v => v.UserId == userId).ToList());

    public VideoView AddApproved(Guid owner, bool isPublic = true, string caption = "clip") =>
        Add(owner, "APPROVED", isPublic, caption);

    public VideoView Add(Guid owner, string moderation, bool isPublic = true, string caption = "clip")
    {
        var video = new VideoView(Guid.NewGuid(), owner, caption, null, null, 0, null, null, isPublic, null, moderation, "DONE");
        Videos[video.VideoId] = video;
        return video;
    }
}
