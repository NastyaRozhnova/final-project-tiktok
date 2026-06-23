using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.UseCases;

public interface IFavouriteService
{
    Task<IReadOnlyList<VideoResponse>> ListVideosAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task RemoveVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SoundResponse>> ListSoundsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddSoundAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default);

    Task RemoveSoundAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default);
}
