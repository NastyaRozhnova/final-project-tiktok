namespace TikTokFeed.Content.Application.Abstractions.Services;

// Внешний сервис модерации (Vision mock)
public interface IVisionModerationClient
{
    Task<VisionVerdict> ModerateAsync(Guid videoId, string videoUrl, string caption, CancellationToken cancellationToken);
}
