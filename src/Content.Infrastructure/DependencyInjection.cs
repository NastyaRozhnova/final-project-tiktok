using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Infrastructure.Persistence;
using TikTokFeed.Content.Infrastructure.Persistence.Repositories;

namespace TikTokFeed.Content.Infrastructure;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1724:Type names should not match namespaces",
    Justification = "Extension methods class for service registration.")]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<ContentDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ISoundRepository, SoundRepository>();
        services.AddScoped<IFavouriteRepository, FavouriteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}