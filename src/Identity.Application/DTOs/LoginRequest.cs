using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Identity.Application.DTOs;

public sealed record LoginRequest(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password);
