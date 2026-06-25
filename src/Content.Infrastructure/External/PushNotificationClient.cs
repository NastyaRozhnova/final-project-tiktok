using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TikTokFeed.Content.Application.Abstractions.Services;

namespace TikTokFeed.Content.Infrastructure.External;

// HTTP-клиент внешнего Push мока
public class PushNotificationClient : IPushNotificationClient
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<PushNotificationClient> _logger;

    public PushNotificationClient(HttpClient httpClient, ILogger<PushNotificationClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> NotifyPublishedAsync(Guid videoId, Guid authorId, CancellationToken cancellationToken)
    {
        try
        {
            var payload = new { video_id = videoId, author_id = authorId, type = "VIDEO_PUBLISHED" };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/notify", payload, cancellationToken);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Push notification sent for video {VideoId}", videoId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Push notification failed for video {VideoId} (swallowed)", videoId);
            return false;
        }
    }
}
