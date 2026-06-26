using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TikTokFeed.Contracts.Grpc.Content;
using TikTokFeed.Contracts.Grpc.Identity;
using TikTokFeed.Contracts.Resilience;
using TikTokFeed.Engagement.Application.Abstractions.Repositories;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.Infrastructure.Grpc;
using TikTokFeed.Engagement.Infrastructure.Persistence;
using TikTokFeed.Engagement.Infrastructure.Persistence.Repositories;

namespace TikTokFeed.Engagement.Infrastructure;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Naming",
    "CA1724:Type names should not match namespaces",
    Justification = "Extension methods class for service registration.")]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<EngagementDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IRepostRepository, RepostRepository>();
        services.AddScoped<IViewRepository, ViewRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        string identityUrl = configuration["Grpc:IdentityUrl"] ?? "http://localhost:7081";
        string contentUrl = configuration["Grpc:ContentUrl"] ?? "http://localhost:9081";

        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
                options.Address = new Uri(identityUrl))
            .AddPolicyHandler((sp, _) => HttpResiliencePolicies.Retry(sp.GetService<ILogger<IdentityGateway>>(), 2))
            .AddPolicyHandler(HttpResiliencePolicies.Timeout());

        services.AddGrpcClient<ContentService.ContentServiceClient>(options =>
                options.Address = new Uri(contentUrl))
            .AddPolicyHandler((sp, _) => HttpResiliencePolicies.Retry(sp.GetService<ILogger<ContentGateway>>(), 2))
            .AddPolicyHandler(HttpResiliencePolicies.Timeout());

        services.AddScoped<IIdentityGateway, IdentityGateway>();
        services.AddScoped<IContentGateway, ContentGateway>();

        return services;
    }
}
