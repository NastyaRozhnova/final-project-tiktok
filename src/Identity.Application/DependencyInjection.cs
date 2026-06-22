using Microsoft.Extensions.DependencyInjection;
using TikTokFeed.Identity.Application.Abstractions.UseCases;
using TikTokFeed.Identity.Application.Services;

namespace TikTokFeed.Identity.Application;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1724:Type names should not match namespaces",
    Justification = "Extension methods class for service registration.")]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
