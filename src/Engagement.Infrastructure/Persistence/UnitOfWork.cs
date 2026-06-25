using TikTokFeed.Engagement.Application.Abstractions.Repositories;

namespace TikTokFeed.Engagement.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly EngagementDbContext _context;

    public UnitOfWork(EngagementDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
