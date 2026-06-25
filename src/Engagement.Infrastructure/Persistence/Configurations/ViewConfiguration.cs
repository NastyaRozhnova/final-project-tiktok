using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Infrastructure.Persistence.Configurations;

public class ViewConfiguration : IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(v => v.Id);
        builder.HasIndex(v => v.VideoId);

        // Идемпотентность ретраев: один ключ от одного пользователя — одна запись.
        builder.HasIndex(v => new { v.UserId, v.IdempotencyKey })
            .IsUnique()
            .HasFilter("\"IdempotencyKey\" IS NOT NULL");
    }
}
