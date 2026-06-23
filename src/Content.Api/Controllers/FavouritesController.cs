using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users/{userId:guid}/favourites")]
[Produces("application/json")]
public class FavouritesController : ControllerBase
{
    private readonly IFavouriteService _favouriteService;

    public FavouritesController(IFavouriteService favouriteService)
    {
        _favouriteService = favouriteService;
    }

    [HttpGet("videos")]
    [ProducesResponseType(typeof(IReadOnlyList<VideoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListVideos(Guid userId, CancellationToken cancellationToken)
    {
        IReadOnlyList<VideoResponse> videos = await _favouriteService.ListVideosAsync(userId, cancellationToken);

        return Ok(videos);
    }

    [HttpPost("videos/{videoId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddVideo(Guid userId, Guid videoId, CancellationToken cancellationToken)
    {
        await _favouriteService.AddVideoAsync(userId, videoId, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("videos/{videoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveVideo(Guid userId, Guid videoId, CancellationToken cancellationToken)
    {
        await _favouriteService.RemoveVideoAsync(userId, videoId, cancellationToken);

        return NoContent();
    }

    [HttpGet("sounds")]
    [ProducesResponseType(typeof(IReadOnlyList<SoundResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListSounds(Guid userId, CancellationToken cancellationToken)
    {
        IReadOnlyList<SoundResponse> sounds = await _favouriteService.ListSoundsAsync(userId, cancellationToken);

        return Ok(sounds);
    }

    [HttpPost("sounds/{soundId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddSound(Guid userId, Guid soundId, CancellationToken cancellationToken)
    {
        await _favouriteService.AddSoundAsync(userId, soundId, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("sounds/{soundId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSound(Guid userId, Guid soundId, CancellationToken cancellationToken)
    {
        await _favouriteService.RemoveSoundAsync(userId, soundId, cancellationToken);

        return NoContent();
    }
}
