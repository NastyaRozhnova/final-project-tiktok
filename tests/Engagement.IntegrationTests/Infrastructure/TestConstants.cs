using System.Collections.Generic;

namespace TikTokFeed.Engagement.IntegrationTests.Infrastructure;

internal static class TestConstants
{
    public const string JwtSecret = "dev-super-secret-shared-key-change-me-please-32bytes!";
    public const string JwtIssuer = "tiktok-identity";
    public const string JwtAudience = "tiktok-clients";

    public static Dictionary<string, string?> JwtConfig => new()
    {
        ["JwtSettings:Secret"] = JwtSecret,
        ["JwtSettings:Issuer"] = JwtIssuer,
        ["JwtSettings:Audience"] = JwtAudience,
        ["JwtSettings:ExpirationMinutes"] = "60",
    };
}
