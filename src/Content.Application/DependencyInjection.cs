using Microsoft.Extensions.DependencyInjection;
using TikTokFeed.Content.Application.Abstractions.UseCases;
using TikTokFeed.Content.Application.Services;

namespace TikTokFeed.Content.Application;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1724:Type names should not match namespaces",
    Justification = "Extension methods class for service registration.")]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<ISoundService, SoundService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<IModerationService, ModerationService>();

        return services;
    }
}
