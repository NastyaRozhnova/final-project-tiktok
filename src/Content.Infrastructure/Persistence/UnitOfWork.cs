using TikTokFeed.Content.Application.Abstractions.Repositories;

namespace TikTokFeed.Content.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContentDbContext _context;

    public UnitOfWork(ContentDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
