namespace TikTokFeed.Identity.Application.Abstractions.Services;

public interface IJwtTokenValidator
{
    JwtValidationResult Validate(string token);
}
