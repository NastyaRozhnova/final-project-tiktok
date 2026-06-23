using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Content.Application.DTOs;

public sealed record CreateVideoRequest(
    [Required]
    [StringLength(2200)]
    string Caption,

    [Required]
    [Url]
    string VideoUrl,

    Guid? SoundId,
    bool IsPublic = true);
