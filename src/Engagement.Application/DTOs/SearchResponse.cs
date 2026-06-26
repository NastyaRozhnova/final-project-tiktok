namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record SearchResponse(
    IReadOnlyList<VideoDto> Videos,
    IReadOnlyList<UserDto> Users,
    int TotalCount);
