namespace TikTokFeed.Engagement.Application.Abstractions.Services;

public interface IIdentityGateway
{
    Task<IReadOnlyList<Guid>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken);

    Task<UserView?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}
