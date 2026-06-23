using TikTokFeed.Content.Application.DTOs;

namespace TikTokFeed.Content.Application.Abstractions.UseCases;

public interface ISoundService
{
    Task<SoundResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
