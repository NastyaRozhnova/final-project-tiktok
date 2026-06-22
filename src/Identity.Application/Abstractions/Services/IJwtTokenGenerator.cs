using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Application.Abstractions.Services;

public interface IJwtTokenGenerator
{
    AuthToken GenerateToken(User user);
}
