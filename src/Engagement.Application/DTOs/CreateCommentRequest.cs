using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record CreateCommentRequest(
    [Required]
    [StringLength(2200)]
    string CommentText,
    Guid? ParentCommentId);
