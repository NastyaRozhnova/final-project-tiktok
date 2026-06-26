using System.Text;
using TikTokFeed.Engagement.Application.Abstractions.Services;
using TikTokFeed.Engagement.Application.Abstractions.UseCases;
using TikTokFeed.Engagement.Application.DTOs;
using TikTokFeed.Engagement.Domain.Exceptions;

namespace TikTokFeed.Engagement.Application.Services;

public class FeedService : IFeedService
{
    private const int MaxLimit = 100;

    private readonly IContentGateway _content;

    private readonly IIdentityGateway _identity;

    private readonly ICurrentUserService _currentUser;

    public FeedService(IContentGateway content, IIdentityGateway identity, ICurrentUserService currentUser)
    {
        _content = content;
        _identity = identity;
        _currentUser = currentUser;
    }

    public async Task<FeedResponse> GetFeedAsync(string? cursor, int limit, CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxLimit);
        Guid userId = _currentUser.UserId;

        IReadOnlyList<Guid> following = await _identity.GetFollowingAsync(userId, cancellationToken);

        var all = new List<VideoView>();
        foreach (Guid authorId in following)
        {
            all.AddRange(await _content.GetVideosByUserAsync(authorId, cancellationToken));
        }

        // Правила №5 и №11: в ленте только одобренные публичные видео.
        var visible = all
            .Where(video => video.IsPublic
                && string.Equals(video.ModerationStatus, "APPROVED", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(video => video.VideoId)
            .Select(ToDto)
            .ToList();

        int offset = DecodeCursor(cursor);
        var page = visible.Skip(offset).Take(limit).ToList();
        int nextOffset = offset + page.Count;
        string? nextCursor = nextOffset < visible.Count ? EncodeCursor(nextOffset) : null;

        return new FeedResponse(page, nextCursor);
    }

    public async Task<SearchResponse> SearchAsync(string? query, string type, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ValidationException("Query parameter 'q' is required");
        }

        var videos = new List<VideoDto>();
        var users = new List<UserDto>();

        if (_currentUser.IsAuthenticated)
        {
            Guid userId = _currentUser.UserId;
            var scope = new List<Guid> { userId };
            scope.AddRange(await _identity.GetFollowingAsync(userId, cancellationToken));
            var distinct = scope.Distinct().ToList();

            if (type is "all" or "video")
            {
                foreach (Guid authorId in distinct)
                {
                    IReadOnlyList<VideoView> byUser = await _content.GetVideosByUserAsync(authorId, cancellationToken);
                    videos.AddRange(byUser
                        .Where(video => video.Caption.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .Select(ToDto));
                }
            }

            if (type is "all" or "user")
            {
                foreach (Guid candidateId in distinct)
                {
                    UserView? user = await _identity.GetUserAsync(candidateId, cancellationToken);
                    if (user is not null && user.Username.Contains(query, StringComparison.OrdinalIgnoreCase))
                    {
                        users.Add(new UserDto(user.UserId, user.Username, user.IsCreator));
                    }
                }
            }
        }

        return new SearchResponse(videos, users, videos.Count + users.Count);
    }

    private static VideoDto ToDto(VideoView video) =>
        new(
            video.VideoId,
            video.UserId,
            video.Caption,
            video.VideoUrl,
            video.ThumbnailUrl,
            video.Duration,
            video.Resolution,
            video.UploadTimestamp,
            video.IsPublic,
            video.SoundId,
            video.ModerationStatus,
            video.ProcessingStatus);

    private static int DecodeCursor(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return 0;
        }

        try
        {
            string raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            return int.TryParse(raw, out int offset) && (offset >= 0) ? offset : 0;
        }
        catch (FormatException)
        {
            return 0;
        }
    }

    private static string EncodeCursor(int offset) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(offset.ToString(System.Globalization.CultureInfo.InvariantCulture)));
}
