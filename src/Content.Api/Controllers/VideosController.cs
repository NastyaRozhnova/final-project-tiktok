using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/videos")]
[Produces("application/json")]
public class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVideoRequest request, CancellationToken cancellationToken)
    {
        VideoResponse video = await _videoService.CreateAsync(request, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, video);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        VideoResponse video = await _videoService.GetByIdAsync(id, cancellationToken);

        return Ok(video);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVideoRequest request, CancellationToken cancellationToken)
    {
        VideoResponse video = await _videoService.UpdateAsync(id, request, cancellationToken);

        return Ok(video);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _videoService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/stats")]
    [ProducesResponseType(typeof(VideoStatsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Stats(Guid id, CancellationToken cancellationToken)
    {
        VideoStatsResponse stats = await _videoService.GetStatsAsync(id, cancellationToken);

        return Ok(stats);
    }
}
