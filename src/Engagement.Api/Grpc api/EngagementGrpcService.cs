using Grpc.Core;
using TikTokFeed.Contracts.Grpc.Engagement;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;

namespace TikTokFeed.Engagement.Api.Grpc;

public class EngagementGrpcService : EngagementService.EngagementServiceBase
{
    private readonly IVideoStatsService _stats;

    public EngagementGrpcService(IVideoStatsService stats)
    {
        _stats = stats;
    }

    public override async Task<VideoStatsProto> GetVideoStats(GetVideoStatsRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        if (!Guid.TryParse(request.VideoId, out Guid videoId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Bad video_id"));
        }

        VideoStats stats = await _stats.GetStatsAsync(videoId, context.CancellationToken);
        return new VideoStatsProto
        {
            VideoId = request.VideoId,
            ViewsCount = stats.Views,
            LikesCount = stats.Likes,
            CommentsCount = stats.Comments,
            RepostsCount = stats.Reposts,
        };
    }
}
