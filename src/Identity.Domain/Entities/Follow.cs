namespace TikTokFeed.Identity.Domain.Entities;

// Подписка одного пользователя на другого
public class Follow
{
    public Guid Id { get; private set; }

    public Guid FollowedId { get; private set; }

    public Guid FollowerId { get; private set; }

    public DateTime FollowDate { get; private set; }

    public Follow(Guid followerId, Guid followedId)
    {
        Id = Guid.NewGuid();
        FollowerId = followerId;
        FollowedId = followedId;
        FollowDate = DateTime.UtcNow;
    }

    private Follow()
    {
    }
}
