using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.DTOs;
using TikTokFeed.Content.Application.Mappings;
using TikTokFeed.Content.Domain.Entities;
using TikTokFeed.Content.Domain.Exceptions;

namespace TikTokFeed.Content.Application.Services;

public class SoundService : ISoundService
{
    private readonly ISoundRepository _sounds;

    public SoundService(ISoundRepository sounds)
    {
        _sounds = sounds;
    }

    public async Task<SoundResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Sound sound = await _sounds.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Sound not found");

        return sound.ToResponse();
    }
}
