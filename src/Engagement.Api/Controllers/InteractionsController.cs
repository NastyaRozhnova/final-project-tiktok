using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;
using TikTokFeed.Engagement.Application.DTOs;

namespace TikTokFeed.Engagement.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/videos/{id:guid}")]
[Produces("application/json")]
public class InteractionsController : ControllerBase
{
    private readonly IInteractionService _interactions;

    public InteractionsController(IInteractionService interactions)
    {
        _interactions = interactions;
    }

    [HttpPost("likes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Like(Guid id, CancellationToken cancellationToken)
    {
        await _interactions.LikeAsync(id, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("likes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unlike(Guid id, CancellationToken cancellationToken)
    {
        await _interactions.UnlikeAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("comments")]
    [ProducesResponseType(typeof(IReadOnlyList<CommentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Comments(Guid id, CancellationToken cancellationToken)
    {
        IReadOnlyList<CommentResponse> comments = await _interactions.GetCommentsAsync(id, cancellationToken);

        return Ok(comments);
    }

    [HttpPost("comments")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
    {
        CommentResponse comment = await _interactions.AddCommentAsync(id, request, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, comment);
    }

    [HttpDelete("comments/{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComment(Guid id, Guid commentId, CancellationToken cancellationToken)
    {
        await _interactions.DeleteCommentAsync(id, commentId, cancellationToken);

        return NoContent();
    }

    [HttpPost("reposts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Repost(Guid id, CancellationToken cancellationToken)
    {
        await _interactions.RepostAsync(id, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("reposts")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unrepost(Guid id, CancellationToken cancellationToken)
    {
        await _interactions.UnrepostAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("views")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> View(
        Guid id,
        [FromBody] RecordViewRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _interactions.RecordViewAsync(id, request, idempotencyKey, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }
}
