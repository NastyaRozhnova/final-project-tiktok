using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.ProfileInfo).HasMaxLength(1000);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
