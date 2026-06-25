using Microsoft.EntityFrameworkCore;
using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence.Repositories;

public class ViewRepository : IViewRepository
{
    private readonly EngagementDbContext _context;

    public ViewRepository(EngagementDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsByKeyAsync(Guid userId, string idempotencyKey, CancellationToken cancellationToken = default) =>
        _context.Views.AnyAsync(v => v.UserId == userId && v.IdempotencyKey == idempotencyKey, cancellationToken);

    public Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Views.LongCountAsync(v => v.VideoId == videoId, cancellationToken);

    public void Add(View view) => _context.Views.Add(view);
}
