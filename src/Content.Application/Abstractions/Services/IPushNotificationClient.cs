namespace TikTokFeed.Content.Application.Abstractions.Services;

// Внешний сервис push-уведомлений (Push mock)
public interface IPushNotificationClient
{
    Task<bool> NotifyPublishedAsync(Guid videoId, Guid authorId, CancellationToken cancellationToken);
}
