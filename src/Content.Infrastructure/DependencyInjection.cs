using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TikTokFeed.Content.Application.Abstractions.Repositories;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Infrastructure.External;
using TikTokFeed.Content.Infrastructure.Grpc;
using TikTokFeed.Content.Infrastructure.Persistence;
using TikTokFeed.Content.Infrastructure.Persistence.Repositories;
using TikTokFeed.Content.Infrastructure.Services;
using TikTokFeed.Contracts.Grpc.Engagement;
using TikTokFeed.Contracts.Resilience;

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

        services.AddSingleton<IVideoProcessingQueue, VideoProcessingQueue>();

        string visionUrl = configuration["ExternalServices:VisionUrl"] ?? "http://localhost:8090";
        string pushUrl = configuration["ExternalServices:PushUrl"] ?? "http://localhost:8091";
        string engagementUrl = configuration["Grpc:EngagementUrl"] ?? "http://localhost:6081";

        services.AddHttpClient<IVisionModerationClient, VisionModerationClient>(client =>
            client.BaseAddress = new Uri(visionUrl));

        services.AddHttpClient<IPushNotificationClient, PushNotificationClient>(client =>
            client.BaseAddress = new Uri(pushUrl));

        services.AddGrpcClient<EngagementService.EngagementServiceClient>(options =>
                options.Address = new Uri(engagementUrl))
            .AddPolicyHandler((sp, _) => HttpResiliencePolicies.Retry(sp.GetService<ILogger<EngagementStatsClient>>(), 2))
            .AddPolicyHandler(HttpResiliencePolicies.Timeout());

        services.AddScoped<IEngagementStatsClient, EngagementStatsClient>();

        return services;
    }
}