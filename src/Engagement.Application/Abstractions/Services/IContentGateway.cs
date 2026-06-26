namespace TikTokFeed.Engagement.Application.Abstractions.Services;

public interface IContentGateway
{
    Task<VideoView?> GetVideoAsync(Guid videoId, CancellationToken cancellationToken);

    Task<IReadOnlyList<VideoView>> GetVideosByUserAsync(Guid userId, CancellationToken cancellationToken);
}
