namespace TikTokFeed.Engagement.Domain.Entities;

// Репост
public class Repost
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid VideoId { get; private set; }

    public DateTime RepostTimestamp { get; private set; }

    public Repost(Guid userId, Guid videoId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VideoId = videoId;
        RepostTimestamp = DateTime.UtcNow;
    }

    private Repost()
    {
    }
}
