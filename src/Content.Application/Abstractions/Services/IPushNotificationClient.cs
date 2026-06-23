namespace TikTokFeed.Content.Application.Abstractions.Services;

/// <summary>Внешний сервис push-уведомлений (Push mock), вызывается по HTTP.</summary>
public interface IPushNotificationClient
{
    Task<bool> NotifyPublishedAsync(Guid videoId, Guid authorId, CancellationToken cancellationToken);
}
