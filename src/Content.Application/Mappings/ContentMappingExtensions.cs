using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Application.Mappings;

public static class ContentMappingExtensions
{
    public static VideoResponse ToResponse(this Video video)
    {
        ArgumentNullException.ThrowIfNull(video);

        return new VideoResponse(
            video.Id,
            video.UserId,
            video.Caption,
            video.VideoUrl,
            video.ThumbnailUrl,
            video.Duration,
            video.Resolution,
            video.UploadTimestamp,
            video.IsPublic,
            video.SoundId,
            video.ModerationStatus,
            video.ProcessingStatus);
    }

    public static SoundResponse ToResponse(this Sound sound)
    {
        ArgumentNullException.ThrowIfNull(sound);

        return new SoundResponse(
            sound.Id,
            sound.SoundName,
            sound.Artist,
            sound.AudioUrl,
            sound.CreatedAt,
            sound.CreatedByVideoId);
    }
}
