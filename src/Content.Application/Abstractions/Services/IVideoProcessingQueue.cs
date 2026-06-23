namespace TikTokFeed.Content.Application.Abstractions.Services;

/// <summary>Очередь фоновой обработки только что загруженных видео.</summary>
public interface IVideoProcessingQueue
{
    ValueTask EnqueueAsync(Guid videoId);

    IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken);
}
