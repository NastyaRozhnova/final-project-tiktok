using Microsoft.EntityFrameworkCore;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence;

public class EngagementDbContext : DbContext
{
    public EngagementDbContext(DbContextOptions<EngagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Like> Likes => Set<Like>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<Repost> Reposts => Set<Repost>();

    public DbSet<View> Views => Set<View>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EngagementDbContext).Assembly);
    }
}
