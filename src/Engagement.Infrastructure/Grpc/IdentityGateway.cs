using Grpc.Core;
using TikTokFeed.Contracts.Grpc.Identity;
using TikTokFeed.Engagement.Application.Abstractions.Services;

namespace TikTokFeed.Engagement.Infrastructure.Grpc;

public class IdentityGateway : IIdentityGateway
{
    private readonly IdentityService.IdentityServiceClient _client;

    public IdentityGateway(IdentityService.IdentityServiceClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<Guid>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken)
    {
        FollowingProto response = await _client.GetFollowingAsync(
            new GetFollowingRequest { UserId = userId.ToString() }, cancellationToken: cancellationToken);

        var result = new List<Guid>(response.FollowingIds.Count);
        foreach (string id in response.FollowingIds)
        {
            if (Guid.TryParse(id, out Guid parsed))
            {
                result.Add(parsed);
            }
        }

        return result;
    }

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken)
    {
        // Намеренный явный кросс-сервисный вызов проверки токена (Этап 4, §5.1 trade-off).
        TokenValidationProto response = await _client.ValidateTokenAsync(
            new ValidateTokenRequest { Token = token }, cancellationToken: cancellationToken);
        return response.IsValid;
    }

    public async Task<UserView?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            UserProto user = await _client.GetUserAsync(
                new GetUserRequest { UserId = userId.ToString() }, cancellationToken: cancellationToken);
            return new UserView(Guid.Parse(user.UserId), user.Username, user.IsCreator);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}
