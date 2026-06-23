using Microsoft.EntityFrameworkCore;
using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Infrastructure.Persistence.Repositories;

public class SoundRepository : ISoundRepository
{
    private readonly ContentDbContext _context;

    public SoundRepository(ContentDbContext context)
    {
        _context = context;
    }

    public Task<Sound?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Sounds.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Sound>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default) =>
        await _context.Sounds.Where(s => ids.Contains(s.Id)).ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Sounds.AnyAsync(s => s.Id == id, cancellationToken);
}
