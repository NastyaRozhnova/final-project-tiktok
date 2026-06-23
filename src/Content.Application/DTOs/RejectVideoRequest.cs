using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Content.Application.DTOs;

public sealed record RejectVideoRequest(
    [Required]
    [StringLength(500)]
    string Reason);
