using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TikTokFeed.Contracts.Auth;
using TikTokFeed.Identity.Application.Abstractions.Services;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private const int DefaultExpirationMinutes = 60;

    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthToken GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        IConfigurationSection settings = _configuration.GetSection("JwtSettings");
        string secret = settings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");
        int expirationMinutes = int.Parse(
            settings["ExpirationMinutes"] ?? DefaultExpirationMinutes.ToString(CultureInfo.InvariantCulture),
            CultureInfo.InvariantCulture);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtClaimNames.Subject, user.Id.ToString()),
            new(JwtClaimNames.Username, user.Username),
        };

        if (user.IsModerator)
        {
            claims.Add(new Claim(JwtClaimNames.Role, JwtClaimNames.RoleModerator));
        }

        var token = new JwtSecurityToken(
            issuer: settings["Issuer"],
            audience: settings["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        string encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthToken(encoded, expirationMinutes * 60);
    }
}
