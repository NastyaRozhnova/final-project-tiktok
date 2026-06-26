using TikTokFeed.Engagement.Application.DTOs;

namespace TikTokFeed.Engagement.Application.Abstractions.UseCases;

public interface IFeedService
{
    Task<FeedResponse> GetFeedAsync(string? cursor, int limit, CancellationToken cancellationToken = default);

    Task<SearchResponse> SearchAsync(string? query, string type, CancellationToken cancellationToken = default);
}
