using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Identity.Application.DTOs;

public sealed record UpdateUserRequest(
    [StringLength(50, MinimumLength = 3)]
    string? Username,
    string? ProfileInfo,
    [Url]
    string? Avatar);
