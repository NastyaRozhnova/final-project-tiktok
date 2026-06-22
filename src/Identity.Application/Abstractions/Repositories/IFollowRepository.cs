using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Application.Abstractions.Repositories;

public interface IFollowRepository
{
    Task<Follow?> GetAsync(Guid followerId, Guid followedId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid followerId, Guid followedId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guid>> GetFollowerIdsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guid>> GetFollowingIdsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guid>> GetAllFollowingIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<int> CountFollowersAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<int> CountFollowingAsync(Guid userId, CancellationToken cancellationToken = default);

    void Add(Follow follow);

    void Remove(Follow follow);
}
