using TikTokFeed.Identity.Application.Abstractions.Repositories;

namespace TikTokFeed.Identity.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IdentityDbContext _context;

    public UnitOfWork(IdentityDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
