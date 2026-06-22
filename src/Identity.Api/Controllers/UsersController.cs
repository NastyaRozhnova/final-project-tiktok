using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TikTokFeed.Identity.Application.Abstractions.UseCases;
using TikTokFeed.Identity.Application.DTOs;

namespace TikTokFeed.Identity.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        UserResponse user = await _userService.GetByIdAsync(id, cancellationToken);

        return Ok(user);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        UserResponse user = await _userService.UpdateAsync(id, request, cancellationToken);

        return Ok(user);
    }

    [HttpPost("{followedId:guid}/follow")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Follow(Guid followedId, CancellationToken cancellationToken)
    {
        await _userService.FollowAsync(followedId, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("{followedId:guid}/follow")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unfollow(Guid followedId, CancellationToken cancellationToken)
    {
        await _userService.UnfollowAsync(followedId, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/followers")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Followers(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<UserResponse> users = await _userService.GetFollowersAsync(id, page, pageSize, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}/following")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Following(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<UserResponse> users = await _userService.GetFollowingAsync(id, page, pageSize, cancellationToken);

        return Ok(users);
    }
}
