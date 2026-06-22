using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Application.Abstractions.Services;
using TikTokFeed.Identity.Application.Abstractions.UseCases;
using TikTokFeed.Identity.Application.DTOs;
using TikTokFeed.Identity.Application.Mappings;
using TikTokFeed.Identity.Domain.Entities;
using TikTokFeed.Identity.Domain.Exceptions;

namespace TikTokFeed.Identity.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    private readonly IFollowRepository _follows;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUser;

    public UserService(
        IUserRepository users,
        IFollowRepository follows,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _users = users;
        _follows = follows;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found");

        return await BuildResponseAsync(user, cancellationToken);
    }

    public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_currentUser.UserId != id)
        {
            throw new ForbiddenException("You can only edit your own profile");
        }

        User user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found");

        if (!string.IsNullOrWhiteSpace(request.Username)
            && !string.Equals(request.Username, user.Username, StringComparison.Ordinal)
            && await _users.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new ConflictException("EMAIL_OR_USERNAME_TAKEN", "Username already taken");
        }

        user.UpdateProfile(request.Username, request.ProfileInfo, request.Avatar);
        _users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await BuildResponseAsync(user, cancellationToken);
    }

    public async Task FollowAsync(Guid followedId, CancellationToken cancellationToken = default)
    {
        Guid followerId = _currentUser.UserId;
        if (followerId == followedId)
        {
            throw new ConflictException("CANNOT_FOLLOW_SELF", "You cannot follow yourself");
        }

        _ = await _users.GetByIdAsync(followedId, cancellationToken)
            ?? throw new NotFoundException("User not found");

        if (await _follows.ExistsAsync(followerId, followedId, cancellationToken))
        {
            throw new ConflictException("ALREADY_FOLLOWING", "Already following this user");
        }

        _follows.Add(new Follow(followerId, followedId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnfollowAsync(Guid followedId, CancellationToken cancellationToken = default)
    {
        Follow follow = await _follows.GetAsync(_currentUser.UserId, followedId, cancellationToken)
            ?? throw new NotFoundException("Follow relation not found");

        _follows.Remove(follow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserResponse>> GetFollowersAsync(Guid id, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Guid> ids = await _follows.GetFollowerIdsAsync(id, page, pageSize, cancellationToken);
        return await LoadUsersAsync(ids, cancellationToken);
    }

    public async Task<IReadOnlyList<UserResponse>> GetFollowingAsync(Guid id, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Guid> ids = await _follows.GetFollowingIdsAsync(id, page, pageSize, cancellationToken);
        return await LoadUsersAsync(ids, cancellationToken);
    }

    private async Task<IReadOnlyList<UserResponse>> LoadUsersAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken)
    {
        IReadOnlyList<User> users = await _users.GetByIdsAsync(ids, cancellationToken);
        var result = new List<UserResponse>(users.Count);
        foreach (User user in users)
        {
            result.Add(await BuildResponseAsync(user, cancellationToken));
        }

        return result;
    }

    private async Task<UserResponse> BuildResponseAsync(User user, CancellationToken cancellationToken)
    {
        int followers = await _follows.CountFollowersAsync(user.Id, cancellationToken);
        int following = await _follows.CountFollowingAsync(user.Id, cancellationToken);
        return user.ToResponse(followers, following);
    }
}
