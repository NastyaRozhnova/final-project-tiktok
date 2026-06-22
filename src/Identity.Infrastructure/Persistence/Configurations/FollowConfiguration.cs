using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Persistence.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(f => f.Id);

        // Бизнес-правило: подписаться на пользователя можно только один раз.
        builder.HasIndex(f => new { f.FollowerId, f.FollowedId }).IsUnique();
        builder.HasIndex(f => f.FollowedId);
    }
}
