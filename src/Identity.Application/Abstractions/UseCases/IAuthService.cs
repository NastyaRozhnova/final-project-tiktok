using TikTokFeed.Identity.Application.DTOs;

namespace TikTokFeed.Identity.Application.Abstractions.UseCases;

public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
