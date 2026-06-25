using Microsoft.EntityFrameworkCore;
using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly EngagementDbContext _context;

    public LikeRepository(EngagementDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Likes.AnyAsync(l => l.UserId == userId && l.VideoId == videoId, cancellationToken);

    public Task<Like?> GetAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.VideoId == videoId, cancellationToken);

    public Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Likes.LongCountAsync(l => l.VideoId == videoId, cancellationToken);

    public void Add(Like entity) => _context.Likes.Add(entity);

    public void Remove(Like entity) => _context.Likes.Remove(entity);
}
