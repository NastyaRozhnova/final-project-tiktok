namespace TikTokFeed.Identity.Application.DTOs;

public sealed record UserResponse(
    Guid UserId,
    string Username,
    string Email,
    string? ProfileInfo,
    string? Avatar,
    DateTime RegistrationDate,
    bool IsCreator,
    int FollowersCount,
    int FollowingCount);
