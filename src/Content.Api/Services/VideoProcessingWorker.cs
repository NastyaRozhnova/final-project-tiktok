using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Api.Services;

// имитация обработки медиа и автоматическая модерация
public class VideoProcessingWorker : BackgroundService
{
    private readonly IVideoProcessingQueue _queue;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<VideoProcessingWorker> _logger;

    public VideoProcessingWorker(
        IVideoProcessingQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<VideoProcessingWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (Guid videoId in _queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                await ProcessAsync(videoId, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Processing failed for video {VideoId}", videoId);
            }
        }
    }

    private async Task ProcessAsync(Guid videoId, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IVideoRepository videos = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        IVisionModerationClient vision = scope.ServiceProvider.GetRequiredService<IVisionModerationClient>();
        IPushNotificationClient push = scope.ServiceProvider.GetRequiredService<IPushNotificationClient>();

        Video? video = await videos.GetByIdAsync(videoId, cancellationToken);
        if (video is null)
        {
            return;
        }

        video.StartProcessing();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
        video.CompleteProcessing($"{video.VideoUrl}#thumb", "1080x1920", 30);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            VisionVerdict verdict = await vision.ModerateAsync(video.Id, video.VideoUrl, video.Caption, cancellationToken);
            if (verdict.Status == ModerationStatus.Approved)
            {
                video.Approve();
            }
            else
            {
                video.Reject(verdict.Reason ?? "Rejected by moderation");
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Vision mock unavailable for {VideoId}; leaving moderation PENDING", video.Id);
            return;
        }

        if (video.ModerationStatus == ModerationStatus.Approved && video.IsPublic && !video.PublishNotified)
        {
            await push.NotifyPublishedAsync(video.Id, video.UserId, cancellationToken);
            video.MarkNotified();
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
