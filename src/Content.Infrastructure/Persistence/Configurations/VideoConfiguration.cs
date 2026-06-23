using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Caption).IsRequired().HasMaxLength(2200);
        builder.Property(v => v.VideoUrl).IsRequired();

        builder.Property(v => v.ModerationStatus)
            .HasConversion(value => value.ToString(), value => Enum.Parse<ModerationStatus>(value))
            .HasMaxLength(20);

        builder.Property(v => v.ProcessingStatus)
            .HasConversion(value => value.ToString(), value => Enum.Parse<ProcessingStatus>(value))
            .HasMaxLength(20);

        builder.HasIndex(v => v.UserId);
        builder.HasIndex(v => v.ModerationStatus);
    }
}
