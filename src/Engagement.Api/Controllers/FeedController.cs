using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;
using TikTokFeed.Engagement.Application.DTOs;

namespace TikTokFeed.Engagement.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1")]
[Produces("application/json")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet("feed")]
    [ProducesResponseType(typeof(FeedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Feed(
        [FromQuery] string? cursor,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        FeedResponse feed = await _feedService.GetFeedAsync(cursor, limit, cancellationToken);

        return Ok(feed);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] string type = "all",
        CancellationToken cancellationToken = default)
    {
        SearchResponse result = await _feedService.SearchAsync(q, type, cancellationToken);

        return Ok(result);
    }
}
