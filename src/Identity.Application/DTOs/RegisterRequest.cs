using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Identity.Application.DTOs;

public sealed record RegisterRequest(
    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Username,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(8)]
    string Password);
