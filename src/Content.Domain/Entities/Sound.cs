namespace TikTokFeed.Content.Domain.Entities;

// Аудиодорожка, переиспользуемая разными видео
public class Sound
{
    public Guid Id { get; private set; }

    public string SoundName { get; private set; } = string.Empty;

    public string? Artist { get; private set; }

    public string? AudioUrl { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid? CreatedByVideoId { get; private set; }

    public Sound(Guid id, string soundName, string? artist, string? audioUrl, Guid? createdByVideoId)
    {
        Id = id;
        SoundName = soundName;
        Artist = artist;
        AudioUrl = audioUrl;
        CreatedByVideoId = createdByVideoId;
        CreatedAt = DateTime.UtcNow;
    }

    private Sound()
    {
    }
}
