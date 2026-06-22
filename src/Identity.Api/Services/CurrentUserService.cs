using TikTokFeed.Contracts.Auth;
using TikTokFeed.Identity.Application.Abstractions.Services;

namespace TikTokFeed.Identity.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            string? subject = _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaimNames.Subject)?.Value;
            return Guid.TryParse(subject, out Guid id) ? id : Guid.Empty;
        }
    }
}
