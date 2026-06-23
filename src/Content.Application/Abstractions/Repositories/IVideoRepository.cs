using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Application.Abstractions.Repositories;

public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Video>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Video>> GetPendingAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Video>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Video video);

    void Update(Video video);

    void Remove(Video video);
}
