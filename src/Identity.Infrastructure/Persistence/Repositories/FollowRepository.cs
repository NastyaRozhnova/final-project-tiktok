using Microsoft.EntityFrameworkCore;
using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Persistence.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly IdentityDbContext _context;

    public FollowRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public Task<Follow?> GetAsync(Guid followerId, Guid followedId, CancellationToken cancellationToken = default) =>
        _context.Follows.FirstOrDefaultAsync(
            f => f.FollowerId == followerId && f.FollowedId == followedId, cancellationToken);

    public Task<bool> ExistsAsync(Guid followerId, Guid followedId, CancellationToken cancellationToken = default) =>
        _context.Follows.AnyAsync(
            f => f.FollowerId == followerId && f.FollowedId == followedId, cancellationToken);

    public async Task<IReadOnlyList<Guid>> GetFollowerIdsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default) =>
        await _context.Follows
            .Where(f => f.FollowedId == userId)
            .OrderBy(f => f.FollowDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => f.FollowerId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Guid>> GetFollowingIdsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default) =>
        await _context.Follows
            .Where(f => f.FollowerId == userId)
            .OrderBy(f => f.FollowDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => f.FollowedId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Guid>> GetAllFollowingIdsAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowedId)
            .ToListAsync(cancellationToken);

    public Task<int> CountFollowersAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _context.Follows.CountAsync(f => f.FollowedId == userId, cancellationToken);

    public Task<int> CountFollowingAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _context.Follows.CountAsync(f => f.FollowerId == userId, cancellationToken);

    public void Add(Follow follow) => _context.Follows.Add(follow);

    public void Remove(Follow follow) => _context.Follows.Remove(follow);
}
