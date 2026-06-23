namespace TikTokFeed.Content.Application.Abstractions.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }

    bool IsAuthenticated { get; }

    bool IsModerator { get; }
}
