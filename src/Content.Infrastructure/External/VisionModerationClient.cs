using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TikTokFeed.Content.Application.Abstractions.Services;
using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Infrastructure.External;

// HTTP-клиент внешнего Vision/Moderation мока
public class VisionModerationClient : IVisionModerationClient
{
    private readonly HttpClient _httpClient;

    private readonly ILogger<VisionModerationClient> _logger;

    public VisionModerationClient(HttpClient httpClient, ILogger<VisionModerationClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<VisionVerdict> ModerateAsync(Guid videoId, string videoUrl, string caption, CancellationToken cancellationToken)
    {
        var payload = new { video_id = videoId, video_url = videoUrl, caption };
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/moderate", payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        VisionResponse body = await response.Content.ReadFromJsonAsync<VisionResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Empty vision response");

        ModerationStatus status = string.Equals(body.Status, "APPROVED", StringComparison.OrdinalIgnoreCase)
            ? ModerationStatus.Approved
            : ModerationStatus.Rejected;

        _logger.LogInformation("Vision verdict for {VideoId}: {Status}", videoId, status);
        return new VisionVerdict(status, body.Reason);
    }

    private sealed record VisionResponse(string Status, string? Reason);
}
