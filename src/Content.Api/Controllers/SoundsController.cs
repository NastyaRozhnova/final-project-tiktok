using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/sounds")]
[Produces("application/json")]
public class SoundsController : ControllerBase
{
    private readonly ISoundService _soundService;

    public SoundsController(ISoundService soundService)
    {
        _soundService = soundService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SoundResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        SoundResponse sound = await _soundService.GetByIdAsync(id, cancellationToken);

        return Ok(sound);
    }
}
