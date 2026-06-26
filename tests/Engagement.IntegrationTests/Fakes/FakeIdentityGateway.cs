using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TikTokFeed.Engagement.Application.Abstractions.Services;

namespace TikTokFeed.Engagement.IntegrationTests.Fakes;

public sealed class FakeIdentityGateway : IIdentityGateway
{
    public Dictionary<Guid, List<Guid>> Following { get; } = new();

    public Dictionary<Guid, UserView> Users { get; } = new();

    public Task<IReadOnlyList<Guid>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken) =>
        Task.FromResult((IReadOnlyList<Guid>)(Following.TryGetValue(userId, out List<Guid>? list) ? list : new List<Guid>()));

    public Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken) => Task.FromResult(true);

    public Task<UserView?> GetUserAsync(Guid userId, CancellationToken cancellationToken) =>
        Task.FromResult(Users.TryGetValue(userId, out UserView? user) ? user : null);
}
