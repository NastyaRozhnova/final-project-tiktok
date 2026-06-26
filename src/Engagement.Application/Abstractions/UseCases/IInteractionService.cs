using TikTokFeed.Engagement.Application.DTOs;

namespace TikTokFeed.Engagement.Application.Abstractions.UseCases;

public interface IInteractionService
{
    Task LikeAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task UnlikeAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CommentResponse>> GetCommentsAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task<CommentResponse> AddCommentAsync(Guid videoId, CreateCommentRequest request, CancellationToken cancellationToken = default);

    Task DeleteCommentAsync(Guid videoId, Guid commentId, CancellationToken cancellationToken = default);

    Task RepostAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task UnrepostAsync(Guid videoId, CancellationToken cancellationToken = default);

    Task RecordViewAsync(Guid videoId, RecordViewRequest request, string? idempotencyKey, CancellationToken cancellationToken = default);
}
