using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Content.Application.DTOs;

public sealed record UpdateVideoRequest(
    [StringLength(2200)]
    string? Caption,
    bool? IsPublic);
