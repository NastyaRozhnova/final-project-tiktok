using Microsoft.EntityFrameworkCore;
using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Infrastructure.Persistence.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly ContentDbContext _context;

    public VideoRepository(ContentDbContext context)
    {
        _context = context;
    }

    public Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Videos.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Video>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default) =>
        await _context.Videos.Where(v => ids.Contains(v.Id)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Video>> GetPendingAsync(CancellationToken cancellationToken = default) =>
        await _context.Videos
            .Where(v => v.ModerationStatus == ModerationStatus.Pending)
            .OrderBy(v => v.UploadTimestamp)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Video>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await _context.Videos.Where(v => v.UserId == userId).ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Videos.AnyAsync(v => v.Id == id, cancellationToken);

    public void Add(Video video) => _context.Videos.Add(video);

    public void Update(Video video) => _context.Videos.Update(video);

    public void Remove(Video video) => _context.Videos.Remove(video);
}
