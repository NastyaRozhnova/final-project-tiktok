using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/moderation")]
[Produces("application/json")]
public class ModerationController : ControllerBase
{
    private readonly IModerationService _moderationService;

    public ModerationController(IModerationService moderationService)
    {
        _moderationService = moderationService;
    }

    [HttpGet("videos/pending")]
    [ProducesResponseType(typeof(IReadOnlyList<VideoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Pending(CancellationToken cancellationToken)
    {
        IReadOnlyList<VideoResponse> pending = await _moderationService.GetPendingAsync(cancellationToken);

        return Ok(pending);
    }

    [HttpPut("videos/{id:guid}/approve")]
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        VideoResponse video = await _moderationService.ApproveAsync(id, cancellationToken);

        return Ok(video);
    }

    [HttpPut("videos/{id:guid}/reject")]
    [ProducesResponseType(typeof(VideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectVideoRequest request, CancellationToken cancellationToken)
    {
        VideoResponse video = await _moderationService.RejectAsync(id, request, cancellationToken);

        return Ok(video);
    }
}
