using TikTokFeed.Content.Domain.Entities;

namespace TikTokFeed.Content.Application.Abstractions.Repositories;

public interface IFavouriteRepository
{
    Task<bool> VideoExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<FavouriteVideo?> GetVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guid>> GetFavouriteVideoIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> SoundExistsAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default);

    Task<FavouriteSound?> GetSoundAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guid>> GetFavouriteSoundIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    void AddVideo(FavouriteVideo favourite);

    void RemoveVideo(FavouriteVideo favourite);

    void AddSound(FavouriteSound favourite);

    void RemoveSound(FavouriteSound favourite);
}
