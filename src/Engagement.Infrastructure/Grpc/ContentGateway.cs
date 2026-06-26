using Grpc.Core;
using TikTokFeed.Contracts.Grpc.Content;
using TikTokFeed.Engagement.Application.Abstractions.Services;

namespace TikTokFeed.Engagement.Infrastructure.Grpc;

public class ContentGateway : IContentGateway
{
    private readonly ContentService.ContentServiceClient _client;

    public ContentGateway(ContentService.ContentServiceClient client)
    {
        _client = client;
    }

    public async Task<VideoView?> GetVideoAsync(Guid videoId, CancellationToken cancellationToken)
    {
        try
        {
            VideoProto video = await _client.GetVideoAsync(
                new GetVideoRequest { VideoId = videoId.ToString() }, cancellationToken: cancellationToken);
            return Map(video);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IReadOnlyList<VideoView>> GetVideosByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        VideosProto response = await _client.GetVideosByUserAsync(
            new GetVideosByUserRequest { UserId = userId.ToString() }, cancellationToken: cancellationToken);
        return response.Videos.Select(Map).ToList();
    }

    private static VideoView Map(VideoProto video) => new(
        Guid.Parse(video.VideoId),
        Guid.Parse(video.UserId),
        video.Caption,
        string.IsNullOrEmpty(video.VideoUrl) ? null : video.VideoUrl,
        string.IsNullOrEmpty(video.ThumbnailUrl) ? null : video.ThumbnailUrl,
        video.Duration,
        string.IsNullOrEmpty(video.Resolution) ? null : video.Resolution,
        string.IsNullOrEmpty(video.UploadTimestamp) ? null : DateTime.Parse(video.UploadTimestamp),
        video.IsPublic,
        string.IsNullOrEmpty(video.SoundId) ? null : Guid.Parse(video.SoundId),
        video.ModerationStatus,
        video.ProcessingStatus);
}
