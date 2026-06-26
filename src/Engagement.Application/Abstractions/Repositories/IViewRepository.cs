using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Application.Abstractions.Repositories;

public interface IViewRepository
{
    Task<bool> ExistsByKeyAsync(Guid userId, string idempotencyKey, CancellationToken cancellationToken = default);

    Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default);

    void Add(View view);
}
