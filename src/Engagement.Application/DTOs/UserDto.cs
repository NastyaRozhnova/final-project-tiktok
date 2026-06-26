namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record UserDto(Guid UserId, string Username, bool IsCreator);
