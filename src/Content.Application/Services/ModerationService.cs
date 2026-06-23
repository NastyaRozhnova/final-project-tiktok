using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Content.Application.Mappings;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Exceptions;

namespace TikTokFeed.Content.Application.Services;

public class ModerationService : IModerationService
{
    private readonly IVideoRepository _videos;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUser;

    private readonly IPushNotificationClient _push;

    public ModerationService(
        IVideoRepository videos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IPushNotificationClient push)
    {
        _videos = videos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _push = push;
    }

    public async Task<IReadOnlyList<VideoResponse>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        EnsureModerator();
        IReadOnlyList<Video> pending = await _videos.GetPendingAsync(cancellationToken);
        return pending.Select(video => video.ToResponse()).ToList();
    }

    public async Task<VideoResponse> ApproveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EnsureModerator();
        Video video = await _videos.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        video.Approve();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (video.IsPublic && !video.PublishNotified)
        {
            await _push.NotifyPublishedAsync(video.Id, video.UserId, cancellationToken);
            video.MarkNotified();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return video.ToResponse();
    }

    public async Task<VideoResponse> RejectAsync(Guid id, RejectVideoRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        EnsureModerator();

        Video video = await _videos.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        video.Reject(request.Reason);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return video.ToResponse();
    }

    private void EnsureModerator()
    {
        if (!_currentUser.IsModerator)
        {
            throw new ForbiddenException("Moderator role required");
        }
    }
}
