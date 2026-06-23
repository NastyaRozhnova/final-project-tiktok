using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Application.Abstractions.Repositories;

public interface ISoundRepository
{
    Task<Sound?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Sound>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
