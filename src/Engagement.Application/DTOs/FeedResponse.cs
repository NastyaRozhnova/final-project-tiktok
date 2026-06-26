namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record FeedResponse(IReadOnlyList<VideoDto> Videos, string? NextCursor);
