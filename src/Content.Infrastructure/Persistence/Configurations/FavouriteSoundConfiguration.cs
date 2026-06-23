using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Infrastructure.Persistence.Configurations;

public class FavouriteSoundConfiguration : IEntityTypeConfiguration<FavouriteSound>
{
    public void Configure(EntityTypeBuilder<FavouriteSound> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(f => f.Id);
        builder.HasIndex(f => new { f.UserId, f.SoundId }).IsUnique();
    }
}
