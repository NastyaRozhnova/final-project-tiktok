using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Application.Abstractions.Repositories;

public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetByVideoAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid commentId, Guid videoId, CancellationToken cancellationToken = default);

    Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default);

    void Add(Comment comment);

    void Remove(Comment comment);
}
