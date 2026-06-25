namespace TikTokFeed.Engagement.Domain.Entities;

// Лайк
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "Like — устоявшийся термин предметной области.")]
public class Like
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid VideoId { get; private set; }

    public DateTime LikeTimestamp { get; private set; }

    public Like(Guid userId, Guid videoId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VideoId = videoId;
        LikeTimestamp = DateTime.UtcNow;
    }

    private Like()
    {
    }
}
