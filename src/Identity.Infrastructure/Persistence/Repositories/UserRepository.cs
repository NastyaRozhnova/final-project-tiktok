using Microsoft.EntityFrameworkCore;
using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<IReadOnlyList<User>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default) =>
        await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync(cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        _context.Users.AnyAsync(u => u.Username == username, cancellationToken);

    public void Add(User user) => _context.Users.Add(user);

    public void Update(User user) => _context.Users.Update(user);
}
