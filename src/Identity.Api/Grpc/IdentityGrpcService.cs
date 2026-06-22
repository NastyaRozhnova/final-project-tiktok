using Grpc.Core;
using TikTokFeed.Contracts.Grpc.Identity;
using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Application.Abstractions.Services;
using TikTokFeed.Identity.Domain.Entities;

namespace TikTokFeed.Identity.Api.Grpc;

// gRPC-поверхность Identity: валидация токена и граф подписок для Engagement при построении ленты
public class IdentityGrpcService : IdentityService.IdentityServiceBase
{
    private readonly IUserRepository _users;

    private readonly IFollowRepository _follows;

    private readonly IJwtTokenValidator _tokenValidator;

    public IdentityGrpcService(
        IUserRepository users,
        IFollowRepository follows,
        IJwtTokenValidator tokenValidator)
    {
        _users = users;
        _follows = follows;
        _tokenValidator = tokenValidator;
    }

    public override async Task<UserProto> GetUser(GetUserRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        if (!Guid.TryParse(request.UserId, out Guid id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Bad user_id"));
        }

        User user = await _users.GetByIdAsync(id, context.CancellationToken)
            ?? throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        return new UserProto { UserId = user.Id.ToString(), Username = user.Username, IsCreator = user.IsCreator };
    }

    public override Task<TokenValidationProto> ValidateToken(ValidateTokenRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);

        JwtValidationResult result = _tokenValidator.Validate(request.Token);

        return Task.FromResult(new TokenValidationProto
        {
            IsValid = result.IsValid,
            UserId = result.UserId == Guid.Empty ? string.Empty : result.UserId.ToString(),
        });
    }

    public override async Task<FollowingProto> GetFollowing(GetFollowingRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(context);

        var result = new FollowingProto();
        if (!Guid.TryParse(request.UserId, out Guid id))
        {
            return result;
        }

        IReadOnlyList<Guid> ids = await _follows.GetAllFollowingIdsAsync(id, context.CancellationToken);
        result.FollowingIds.AddRange(ids.Select(followedId => followedId.ToString()));

        return result;
    }
}
