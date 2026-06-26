namespace TikTokFeed.Engagement.Application.Abstractions.Services;

public sealed record VideoView(
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
