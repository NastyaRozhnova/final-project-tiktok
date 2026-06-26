using System.ComponentModel.DataAnnotations;

namespace TikTokFeed.Engagement.Application.DTOs;

public sealed record RecordViewRequest(
    [Range(0, int.MaxValue)]
    int WatchDuration);
