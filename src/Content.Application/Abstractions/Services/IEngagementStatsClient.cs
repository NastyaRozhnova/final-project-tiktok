using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.Services;

// gRPC-клиент к Engagement
public interface IEngagementStatsClient
{
    Task<VideoStatsResponse> GetStatsAsync(Guid videoId, CancellationToken cancellationToken);
}
