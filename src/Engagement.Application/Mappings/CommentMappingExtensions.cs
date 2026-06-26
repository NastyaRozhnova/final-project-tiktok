using TikTokFeed.Engagement.Application.DTOs;
using TikTokFeed.Engagement.Domain.Entities;

namespace TikTokFeed.Engagement.Application.Mappings;

public static class CommentMappingExtensions
{
    public static CommentResponse ToResponse(this Comment comment, int repliesCount)
    {
        ArgumentNullException.ThrowIfNull(comment);

        return new CommentResponse(
            comment.Id,
            comment.VideoId,
            comment.UserId,
            comment.CommentText,
            comment.CommentTimestamp,
            comment.ParentCommentId,
            repliesCount);
    }
}
