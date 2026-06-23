using Microsoft.Extensions.Logging;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Contracts.Grpc.Engagement;

namespace TikTokFeed.Content.Infrastructure.Grpc;

// gRPC-клиент к Engagement
public class EngagementStatsClient : IEngagementStatsClient
{
    private readonly EngagementService.EngagementServiceClient _client;

    private readonly ILogger<EngagementStatsClient> _logger;

    public EngagementStatsClient(
        EngagementService.EngagementServiceClient client,
        ILogger<EngagementStatsClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<VideoStatsResponse> GetStatsAsync(Guid videoId, CancellationToken cancellationToken)
    {
        try
        {
            VideoStatsProto stats = await _client.GetVideoStatsAsync(
                new GetVideoStatsRequest { VideoId = videoId.ToString() }, cancellationToken: cancellationToken);
            return new VideoStatsResponse(
                videoId, stats.ViewsCount, stats.LikesCount, stats.CommentsCount, stats.RepostsCount);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Engagement stats unavailable for {VideoId}; returning zeros", videoId);
            return new VideoStatsResponse(videoId, 0, 0, 0, 0);
        }
    }
}
