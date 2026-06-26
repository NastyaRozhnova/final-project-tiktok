using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;

namespace TikTokFeed.Engagement.Application.Services;

public class VideoStatsService : IVideoStatsService
{
    private readonly ILikeRepository _likes;

    private readonly ICommentRepository _comments;

    private readonly IRepostRepository _reposts;

    private readonly IViewRepository _views;

    public VideoStatsService(
        ILikeRepository likes,
        ICommentRepository comments,
        IRepostRepository reposts,
        IViewRepository views)
    {
        _likes = likes;
        _comments = comments;
        _reposts = reposts;
        _views = views;
    }

    public async Task<VideoStats> GetStatsAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        long views = await _views.CountByVideoAsync(videoId, cancellationToken);
        long likes = await _likes.CountByVideoAsync(videoId, cancellationToken);
        long comments = await _comments.CountByVideoAsync(videoId, cancellationToken);
        long reposts = await _reposts.CountByVideoAsync(videoId, cancellationToken);
        return new VideoStats(views, likes, comments, reposts);
    }
}
