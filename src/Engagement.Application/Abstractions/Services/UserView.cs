namespace TikTokFeed.Engagement.Application.Abstractions.Services;

public sealed record UserView(Guid UserId, string Username, bool IsCreator);
