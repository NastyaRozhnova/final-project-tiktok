using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;
using TikTokFeed.Engagement.Application.DTOs;
using TikTokFeed.Engagement.Application.Mappings;
using TikTokFeed.Engagement.Domain.Entities;
using TikTokFeed.Engagement.Domain.Exceptions;

namespace TikTokFeed.Engagement.Application.Services;

public class InteractionService : IInteractionService
{
    private readonly ILikeRepository _likes;

    private readonly ICommentRepository _comments;

    private readonly IRepostRepository _reposts;

    private readonly IViewRepository _views;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUser;

    private readonly IContentGateway _content;

    private readonly IIdentityGateway _identity;

    public InteractionService(
        ILikeRepository likes,
        ICommentRepository comments,
        IRepostRepository reposts,
        IViewRepository views,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IContentGateway content,
        IIdentityGateway identity)
    {
        _likes = likes;
        _comments = comments;
        _reposts = reposts;
        _views = views;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _content = content;
        _identity = identity;
    }

    public async Task LikeAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        Guid userId = _currentUser.UserId;
        await EnsureInteractableAsync(videoId, userId, cancellationToken);

        if (await _likes.ExistsAsync(userId, videoId, cancellationToken))
        {
            throw new ConflictException("LIKE_ALREADY_EXISTS", "User has already liked this video");
        }

        _likes.Add(new Like(userId, videoId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnlikeAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        Like like = await _likes.GetAsync(_currentUser.UserId, videoId, cancellationToken)
            ?? throw new NotFoundException("Like not found");

        _likes.Remove(like);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CommentResponse>> GetCommentsAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Comment> comments = await _comments.GetByVideoAsync(videoId, cancellationToken);
        var replyCounts = comments
            .Where(comment => comment.ParentCommentId.HasValue)
            .GroupBy(comment => comment.ParentCommentId.GetValueOrDefault())
            .ToDictionary(group => group.Key, group => group.Count());

        return comments
            .Select(comment => comment.ToResponse(replyCounts.GetValueOrDefault(comment.Id)))
            .ToList();
    }

    public async Task<CommentResponse> AddCommentAsync(Guid videoId, CreateCommentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Guid userId = _currentUser.UserId;
        await EnsureInteractableAsync(videoId, userId, cancellationToken);

        if (request.ParentCommentId is { } parentId
            && !await _comments.ExistsAsync(parentId, videoId, cancellationToken))
        {
            throw new NotFoundException("Parent comment not found", "PARENT_COMMENT_NOT_FOUND");
        }

        var comment = new Comment(videoId, userId, request.CommentText, request.ParentCommentId);
        _comments.Add(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return comment.ToResponse(0);
    }

    public async Task DeleteCommentAsync(Guid videoId, Guid commentId, CancellationToken cancellationToken = default)
    {
        Comment comment = await _comments.GetByIdAsync(commentId, cancellationToken)
            ?? throw new NotFoundException("Comment not found");

        if (comment.VideoId != videoId)
        {
            throw new NotFoundException("Comment not found");
        }

        if (comment.UserId != _currentUser.UserId)
        {
            throw new ForbiddenException("You can only delete your own comment");
        }

        _comments.Remove(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RepostAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        Guid userId = _currentUser.UserId;
        await EnsureInteractableAsync(videoId, userId, cancellationToken);

        if (await _reposts.ExistsAsync(userId, videoId, cancellationToken))
        {
            throw new ConflictException("CONFLICT", "Already reposted");
        }

        _reposts.Add(new Repost(userId, videoId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnrepostAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        Repost repost = await _reposts.GetAsync(_currentUser.UserId, videoId, cancellationToken)
            ?? throw new NotFoundException("Repost not found");

        _reposts.Remove(repost);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RecordViewAsync(Guid videoId, RecordViewRequest request, string? idempotencyKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Guid userId = _currentUser.UserId;
        await EnsureInteractableAsync(videoId, userId, cancellationToken);

        // Идемпотентный ретрай (§5.3): один и тот же ключ от пользователя учитывается один раз.
        if (!string.IsNullOrWhiteSpace(idempotencyKey)
            && await _views.ExistsByKeyAsync(userId, idempotencyKey, cancellationToken))
        {
            return;
        }

        _views.Add(new View(
            userId,
            videoId,
            request.WatchDuration,
            string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureInteractableAsync(Guid videoId, Guid requesterId, CancellationToken cancellationToken)
    {
        VideoView video = await _content.GetVideoAsync(videoId, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        if (!string.Equals(video.ModerationStatus, "APPROVED", StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenException("Video has not passed moderation", "VIDEO_NOT_APPROVED");
        }

        if (!video.IsPublic && video.UserId != requesterId)
        {
            IReadOnlyList<Guid> following = await _identity.GetFollowingAsync(requesterId, cancellationToken);
            if (!following.Contains(video.UserId))
            {
                throw new ForbiddenException("This video is private");
            }
        }
    }
}
