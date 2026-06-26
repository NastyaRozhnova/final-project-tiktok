namespace TikTokFeed.Engagement.Application.Abstractions.UseCases;

public interface IVideoStatsService
{
    Task<VideoStats> GetStatsAsync(Guid videoId, CancellationToken cancellationToken = default);
}
