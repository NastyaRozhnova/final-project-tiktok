using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Application.Abstractions.Services;
using TikTokFeed.Identity.Infrastructure.Persistence;
using TikTokFeed.Identity.Infrastructure.Persistence.Repositories;
using TikTokFeed.Identity.Infrastructure.Services;

namespace TikTokFeed.Identity.Infrastructure;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1724:Type names should not match namespaces",
    Justification = "Extension methods class for service registration.")]

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();

        return services;
    }
}
