using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Infrastructure.Persistence.Configurations;

public class SoundConfiguration : IEntityTypeConfiguration<Sound>
{
    public void Configure(EntityTypeBuilder<Sound> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(s => s.Id);
        builder.Property(s => s.SoundName).IsRequired().HasMaxLength(200);
    }
}
