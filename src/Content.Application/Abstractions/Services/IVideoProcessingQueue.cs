namespace TikTokFeed.Content.Application.Abstractions.Services;

// Очередь фоновой обработки только что загруженных видео
public interface IVideoProcessingQueue
{
    ValueTask EnqueueAsync(Guid videoId);

    IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken);
}
