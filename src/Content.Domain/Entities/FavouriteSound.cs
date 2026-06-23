namespace TikTokFeed.Content.Domain.Entities;

// Избранный звук
public class FavouriteSound
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid SoundId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public FavouriteSound(Guid userId, Guid soundId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SoundId = soundId;
        CreatedAt = DateTime.UtcNow;
    }

    private FavouriteSound()
    {
    }
}
