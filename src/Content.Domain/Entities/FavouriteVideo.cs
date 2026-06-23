namespace TikTokFeed.Content.Domain.Entities;

// Избранное видео
public class FavouriteVideo
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid VideoId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public FavouriteVideo(Guid userId, Guid videoId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VideoId = videoId;
        CreatedAt = DateTime.UtcNow;
    }

    private FavouriteVideo()
    {
    }
}
