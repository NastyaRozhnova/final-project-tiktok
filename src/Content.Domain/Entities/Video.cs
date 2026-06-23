using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Domain.Entities;

// Видео — центральная единица контента
public class Video
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string Caption { get; private set; } = string.Empty;

    public string VideoUrl { get; private set; } = string.Empty;

    public string? ThumbnailUrl { get; private set; }

    public int Duration { get; private set; }

    public string? Resolution { get; private set; }

    public DateTime UploadTimestamp { get; private set; }

    public bool IsPublic { get; private set; }

    public Guid? SoundId { get; private set; }

    public ModerationStatus ModerationStatus { get; private set; }

    public ProcessingStatus ProcessingStatus { get; private set; }

    public string? RejectionReason { get; private set; }

    public bool PublishNotified { get; private set; }

    public Video(Guid id, Guid userId, string caption, string videoUrl, Guid? soundId, bool isPublic)
    {
        Id = id;
        UserId = userId;
        Caption = caption;
        VideoUrl = videoUrl;
        SoundId = soundId;
        IsPublic = isPublic;
        UploadTimestamp = DateTime.UtcNow;
        ModerationStatus = ModerationStatus.Pending;
        ProcessingStatus = ProcessingStatus.Pending;
    }

    private Video()
    {
    }

    public void StartProcessing() => ProcessingStatus = ProcessingStatus.Processing;

    public void CompleteProcessing(string thumbnailUrl, string resolution, int duration)
    {
        ProcessingStatus = ProcessingStatus.Done;
        ThumbnailUrl ??= thumbnailUrl;
        Resolution ??= resolution;
        if (Duration == 0)
        {
            Duration = duration;
        }
    }

    public void Approve()
    {
        ModerationStatus = ModerationStatus.Approved;
        RejectionReason = null;
    }

    public void Reject(string reason)
    {
        ModerationStatus = ModerationStatus.Rejected;
        RejectionReason = reason;
    }

    public void MarkNotified() => PublishNotified = true;

    public void UpdateDetails(string? caption, bool? isPublic)
    {
        if (caption is not null)
        {
            Caption = caption;
        }

        if (isPublic.HasValue)
        {
            IsPublic = isPublic.Value;
        }
    }
}
