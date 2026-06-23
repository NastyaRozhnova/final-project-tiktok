using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.UseCases;

public interface IModerationService
{
    Task<IReadOnlyList<VideoResponse>> GetPendingAsync(CancellationToken cancellationToken = default);

    Task<VideoResponse> ApproveAsync(Guid id, CancellationToken cancellationToken = default);

    Task<VideoResponse> RejectAsync(Guid id, RejectVideoRequest request, CancellationToken cancellationToken = default);
}
