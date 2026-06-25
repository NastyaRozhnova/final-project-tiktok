namespace TikTokFeed.Engagement.Domain.Entities;

// Просмотр
public class View
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid VideoId { get; private set; }

    public DateTime ViewTimestamp { get; private set; }

    public int WatchDuration { get; private set; }

    public string? IdempotencyKey { get; private set; }

    public View(Guid userId, Guid videoId, int watchDuration, string? idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VideoId = videoId;
        WatchDuration = watchDuration;
        IdempotencyKey = idempotencyKey;
        ViewTimestamp = DateTime.UtcNow;
    }

    private View()
    {
    }
}
