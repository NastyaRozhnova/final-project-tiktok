using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Application.Abstractions.Repositories;

public interface ILikeRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<Like?> GetAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default);

    void Add(Like entity);

    void Remove(Like entity);
}
