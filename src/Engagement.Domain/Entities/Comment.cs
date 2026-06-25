namespace TikTokFeed.Engagement.Domain.Entities;

// Комментарий
public class Comment
{
    public Guid Id { get; private set; }

    public Guid VideoId { get; private set; }

    public Guid UserId { get; private set; }

    public string CommentText { get; private set; } = string.Empty;

    public DateTime CommentTimestamp { get; private set; }

    public Guid? ParentCommentId { get; private set; }

    public Comment(Guid videoId, Guid userId, string commentText, Guid? parentCommentId)
    {
        Id = Guid.NewGuid();
        VideoId = videoId;
        UserId = userId;
        CommentText = commentText;
        ParentCommentId = parentCommentId;
        CommentTimestamp = DateTime.UtcNow;
    }

    private Comment()
    {
    }
}
