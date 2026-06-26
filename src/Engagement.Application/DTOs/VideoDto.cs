namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record VideoDto(
    Guid VideoId,
    Guid UserId,
    string Caption,
    string? VideoUrl,
    string? ThumbnailUrl,
    int Duration,
    string? Resolution,
    DateTime? UploadTimestamp,
    bool IsPublic,
    Guid? SoundId,
    string ModerationStatus,
    string ProcessingStatus);
