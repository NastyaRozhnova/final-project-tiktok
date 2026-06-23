using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Content.Application.Mappings;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Exceptions;

namespace TikTokFeed.Content.Application.Services;

public class FavouriteService : IFavouriteService
{
    private readonly IFavouriteRepository _favourites;

    private readonly IVideoRepository _videos;

    private readonly ISoundRepository _sounds;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUser;

    public FavouriteService(
        IFavouriteRepository favourites,
        IVideoRepository videos,
        ISoundRepository sounds,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _favourites = favourites;
        _videos = videos;
        _sounds = sounds;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<VideoResponse>> ListVideosAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        IReadOnlyList<Guid> ids = await _favourites.GetFavouriteVideoIdsAsync(userId, cancellationToken);
        IReadOnlyList<Video> videos = await _videos.GetByIdsAsync(ids, cancellationToken);
        return videos.Select(video => video.ToResponse()).ToList();
    }

    public async Task AddVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        if (!await _videos.ExistsAsync(videoId, cancellationToken))
        {
            throw new NotFoundException("Video not found");
        }

        if (await _favourites.VideoExistsAsync(userId, videoId, cancellationToken))
        {
            throw new ConflictException("ALREADY_FAVOURITE", "Video already in favourites");
        }

        _favourites.AddVideo(new FavouriteVideo(userId, videoId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        FavouriteVideo favourite = await _favourites.GetVideoAsync(userId, videoId, cancellationToken)
            ?? throw new NotFoundException("Not found in favourites");

        _favourites.RemoveVideo(favourite);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SoundResponse>> ListSoundsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        IReadOnlyList<Guid> ids = await _favourites.GetFavouriteSoundIdsAsync(userId, cancellationToken);
        IReadOnlyList<Sound> sounds = await _sounds.GetByIdsAsync(ids, cancellationToken);
        return sounds.Select(sound => sound.ToResponse()).ToList();
    }

    public async Task AddSoundAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        if (!await _sounds.ExistsAsync(soundId, cancellationToken))
        {
            throw new NotFoundException("Sound not found");
        }

        if (await _favourites.SoundExistsAsync(userId, soundId, cancellationToken))
        {
            throw new ConflictException("ALREADY_FAVOURITE", "Sound already in favourites");
        }

        _favourites.AddSound(new FavouriteSound(userId, soundId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSoundAsync(Guid userId, Guid soundId, CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId);
        FavouriteSound favourite = await _favourites.GetSoundAsync(userId, soundId, cancellationToken)
            ?? throw new NotFoundException("Not found in favourites");

        _favourites.RemoveSound(favourite);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private void EnsureSelf(Guid userId)
    {
        if (_currentUser.UserId != userId)
        {
            throw new ForbiddenException("You can only manage your own favourites");
        }
    }
}
