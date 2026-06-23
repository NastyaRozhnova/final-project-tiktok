using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Application.DTOs;

public sealed record VideoResponse(
    Guid VideoId,
    Guid UserId,
    string Caption,
    string VideoUrl,
    string? ThumbnailUrl,
    int Duration,
    string? Resolution,
    DateTime UploadTimestamp,
    bool IsPublic,
    Guid? SoundId,
    ModerationStatus ModerationStatus,
    ProcessingStatus ProcessingStatus);
