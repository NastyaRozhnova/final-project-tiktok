namespace TikTokFeed.Identity.Application.Abstractions.Services;

public sealed record AuthToken(string Token, int ExpiresInSeconds);
