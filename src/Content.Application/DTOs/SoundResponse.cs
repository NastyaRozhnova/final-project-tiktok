namespace TikTokFeed.Content.Application.DTOs;

public sealed record SoundResponse(
    Guid SoundId,
    string SoundName,
    string? Artist,
    string? AudioUrl,
    DateTime CreatedAt,
    Guid? CreatedByVideoId);
