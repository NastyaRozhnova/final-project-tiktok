using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.Services;

/// <summary>gRPC-клиент к Engagement для честных счётчиков видео (§5.1).</summary>
public interface IEngagementStatsClient
{
    Task<VideoStatsResponse> GetStatsAsync(Guid videoId, CancellationToken cancellationToken);
}
