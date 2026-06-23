using TikTokFeed.Content.Domain.Enums;

namespace TikTokFeed.Content.Application.Abstractions.Services;

public sealed record VisionVerdict(ModerationStatus Status, string? Reason);
