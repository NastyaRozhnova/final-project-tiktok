namespace TikTokFeed.Content.Application.Abstractions.Services;

/// <summary>Внешний сервис модерации (Vision mock), вызывается по HTTP.</summary>
public interface IVisionModerationClient
{
    Task<VisionVerdict> ModerateAsync(Guid videoId, string videoUrl, string caption, CancellationToken cancellationToken);
}
