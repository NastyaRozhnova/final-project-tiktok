using Microsoft.EntityFrameworkCore;
using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence.Repositories;

public class RepostRepository : IRepostRepository
{
    private readonly EngagementDbContext _context;

    public RepostRepository(EngagementDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Reposts.AnyAsync(r => r.UserId == userId && r.VideoId == videoId, cancellationToken);

    public Task<Repost?> GetAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Reposts.FirstOrDefaultAsync(r => r.UserId == userId && r.VideoId == videoId, cancellationToken);

    public Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Reposts.LongCountAsync(r => r.VideoId == videoId, cancellationToken);

    public void Add(Repost repost) => _context.Reposts.Add(repost);

    public void Remove(Repost repost) => _context.Reposts.Remove(repost);
}
