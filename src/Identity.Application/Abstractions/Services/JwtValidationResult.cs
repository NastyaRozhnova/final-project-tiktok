namespace TikTokFeed.Identity.Application.Abstractions.Services;

public sealed record JwtValidationResult(bool IsValid, Guid UserId);
