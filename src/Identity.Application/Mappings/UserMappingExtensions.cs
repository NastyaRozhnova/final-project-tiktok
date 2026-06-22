using TikTokFeed.Identity.Application.DTOs;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Application.Mappings;

public static class UserMappingExtensions
{
    public static UserResponse ToResponse(this User user, int followersCount, int followingCount)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserResponse(
            user.Id,
            user.Username,
            user.Email,
            user.ProfileInfo,
            user.Avatar,
            user.RegistrationDate,
            user.IsCreator,
            followersCount,
            followingCount);
    }
}
