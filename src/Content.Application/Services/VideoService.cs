using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Content.Application.Mappings;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Exceptions;

namespace TikTokFeed.Content.Application.Services;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videos;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUser;

    private readonly IVideoProcessingQueue _processingQueue;

    private readonly IEngagementStatsClient _stats;

    public VideoService(
        IVideoRepository videos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IVideoProcessingQueue processingQueue,
        IEngagementStatsClient stats)
    {
        _videos = videos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _processingQueue = processingQueue;
        _stats = stats;
    }

    public async Task<VideoResponse> CreateAsync(CreateVideoRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var video = new Video(
            Guid.NewGuid(),
            _currentUser.UserId,
            request.Caption,
            request.VideoUrl,
            request.SoundId,
            request.IsPublic);

        _videos.Add(video);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Запускаем фоновый конвейер обработки и модерации.
        await _processingQueue.EnqueueAsync(video.Id);

        return video.ToResponse();
    }

    public async Task<VideoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Video video = await _videos.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        return video.ToResponse();
    }

    public async Task<VideoResponse> UpdateAsync(Guid id, UpdateVideoRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Video video = await _videos.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        if (video.UserId != _currentUser.UserId)
        {
            throw new ForbiddenException("You are not the owner of this video");
        }

        video.UpdateDetails(request.Caption, request.IsPublic);
        _videos.Update(video);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return video.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Video video = await _videos.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Video not found");

        if (video.UserId != _currentUser.UserId)
        {
            throw new ForbiddenException("You are not the owner of this video");
        }

        _videos.Remove(video);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<VideoStatsResponse> GetStatsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await _videos.ExistsAsync(id, cancellationToken))
        {
            throw new NotFoundException("Video not found");
        }

        // Честные счётчики берём из Engagement по gRPC (§5.1).
        return await _stats.GetStatsAsync(id, cancellationToken);
    }
}
