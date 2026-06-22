using Microsoft.EntityFrameworkCore;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Persistence;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}
