using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.UseCases;

public interface IVideoService
{
    Task<VideoResponse> CreateAsync(CreateVideoRequest request, CancellationToken cancellationToken = default);

    Task<VideoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<VideoResponse> UpdateAsync(Guid id, UpdateVideoRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<VideoStatsResponse> GetStatsAsync(Guid id, CancellationToken cancellationToken = default);
}
