using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TikTokFeed.Contracts.Auth;
using TikTokFeed.Identity.Application.Abstractions.Services;

namespace TikTokFeed.Identity.Infrastructure.Services;

public class JwtTokenValidator : IJwtTokenValidator
{
    private readonly IConfiguration _configuration;

    public JwtTokenValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtValidationResult Validate(string token)
    {
        IConfigurationSection settings = _configuration.GetSection("JwtSettings");
        string secret = settings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = settings["Issuer"],
            ValidateAudience = true,
            ValidAudience = settings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(5),
        };

        try
        {
            var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out _);
            string? subject = principal.FindFirst(JwtClaimNames.Subject)?.Value;
            return Guid.TryParse(subject, out Guid userId)
                ? new JwtValidationResult(true, userId)
                : new JwtValidationResult(false, Guid.Empty);
        }
        catch (Exception)
        {
            return new JwtValidationResult(false, Guid.Empty);
        }
    }
}
