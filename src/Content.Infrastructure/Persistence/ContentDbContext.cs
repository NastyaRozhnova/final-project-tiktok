using Microsoft.EntityFrameworkCore;
using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Infrastructure.Persistence;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Video> Videos => Set<Video>();

    public DbSet<Sound> Sounds => Set<Sound>();

    public DbSet<FavouriteVideo> FavouriteVideos => Set<FavouriteVideo>();

    public DbSet<FavouriteSound> FavouriteSounds => Set<FavouriteSound>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentDbContext).Assembly);
    }
}
