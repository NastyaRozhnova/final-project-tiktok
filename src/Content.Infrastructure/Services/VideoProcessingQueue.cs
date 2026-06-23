using System.Threading.Channels;
using TikTokFeed.Content.Application.Abstractions.Services;

namespace TikTokFeed.Content.Infrastructure.Services;

public class VideoProcessingQueue : IVideoProcessingQueue
{
    private readonly Channel<Guid> _channel = Channel.CreateUnbounded<Guid>();

    public ValueTask EnqueueAsync(Guid videoId) => _channel.Writer.WriteAsync(videoId);

    public IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken) =>
        _channel.Reader.ReadAllAsync(cancellationToken);
}
