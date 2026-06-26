using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Application.Abstractions.Repositories;

public interface IRepostRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<Repost?> GetAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default);

    void Add(Repost repost);

    void Remove(Repost repost);
}
