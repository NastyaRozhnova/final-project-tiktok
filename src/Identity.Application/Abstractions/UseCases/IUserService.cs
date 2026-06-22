using TikTokFeed.Identity.Application.DTOs;

namespace TikTokFeed.Identity.Application.Abstractions.UseCases;

public interface IUserService
{
    Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task FollowAsync(Guid followedId, CancellationToken cancellationToken = default);

    Task UnfollowAsync(Guid followedId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserResponse>> GetFollowersAsync(Guid id, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserResponse>> GetFollowingAsync(Guid id, int page, int pageSize, CancellationToken cancellationToken = default);
}
