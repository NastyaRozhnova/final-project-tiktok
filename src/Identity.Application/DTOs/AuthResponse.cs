namespace TikTokFeed.Identity.Application.DTOs;

public sealed record AuthResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    AuthUserDto User);
