namespace TikTokFeed.Content.Application.DTOs;

public sealed record VideoStatsResponse(
    Guid VideoId,
    long ViewsCount,
    long LikesCount,
    long CommentsCount,
    long RepostsCount);
