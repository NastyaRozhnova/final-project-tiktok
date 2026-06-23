using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Contracts.Auth;

namespace TikTokFeed.Content.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool IsModerator =>
        _httpContextAccessor.HttpContext?.User.HasClaim(JwtClaimNames.Role, JwtClaimNames.RoleModerator) ?? false;

    public Guid UserId
    {
        get
        {
            string? subject = _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaimNames.Subject)?.Value;
            return Guid.TryParse(subject, out Guid id) ? id : Guid.Empty;
        }
    }
}
