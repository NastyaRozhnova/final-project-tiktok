namespace TikTokFeed.Identity.Application.Abstractions.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }

    bool IsAuthenticated { get; }
}
