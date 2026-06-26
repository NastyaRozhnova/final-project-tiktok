using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TikTokFeed.Contracts.Auth;

namespace TikTokFeed.Engagement.IntegrationTests.Infrastructure;

internal static class TestTokens
{
    public static string Issue(Guid userId, string username = "tester")
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestConstants.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtClaimNames.Subject, userId.ToString()),
            new Claim(JwtClaimNames.Username, username),
        };

        var token = new JwtSecurityToken(
            issuer: TestConstants.JwtIssuer,
            audience: TestConstants.JwtAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
