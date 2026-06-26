namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record CommentResponse(
    Guid CommentId,
    Guid VideoId,
    Guid UserId,
    string CommentText,
    DateTime CommentTimestamp,
    Guid? ParentCommentId,
    int RepliesCount);
