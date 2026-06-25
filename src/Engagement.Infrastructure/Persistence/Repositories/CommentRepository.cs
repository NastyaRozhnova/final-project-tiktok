using Microsoft.EntityFrameworkCore;
using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly EngagementDbContext _context;

    public CommentRepository(EngagementDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Comment>> GetByVideoAsync(Guid videoId, CancellationToken cancellationToken = default) =>
        await _context.Comments
            .Where(c => c.VideoId == videoId)
            .OrderBy(c => c.CommentTimestamp)
            .ToListAsync(cancellationToken);

    public Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken = default) =>
        _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);

    public Task<bool> ExistsAsync(Guid commentId, Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Comments.AnyAsync(c => c.Id == commentId && c.VideoId == videoId, cancellationToken);

    public Task<long> CountByVideoAsync(Guid videoId, CancellationToken cancellationToken = default) =>
        _context.Comments.LongCountAsync(c => c.VideoId == videoId, cancellationToken);

    public void Add(Comment comment) => _context.Comments.Add(comment);

    public void Remove(Comment comment) => _context.Comments.Remove(comment);
}
