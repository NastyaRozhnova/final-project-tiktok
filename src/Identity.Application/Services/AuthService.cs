using TikTokFeed.Identity.Application.Abstractions.Repositories;
using TikTokFeed.Identity.Application.Abstractions.Services;
using TikTokFeed.Identity.Application.Abstractions.UseCases;
using TikTokFeed.Identity.Application.DTOs;
using TikTokFeed.Identity.Application.Mappings;
using TikTokFeed.Identity.Domain.Entities;
using TikTokFeed.Identity.Domain.Exceptions;

namespace TikTokFeed.Identity.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(
        IUserRepository users,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        bool emailTaken = await _users.ExistsByEmailAsync(request.Email, cancellationToken);
        bool usernameTaken = await _users.ExistsByUsernameAsync(request.Username, cancellationToken);
        if (emailTaken || usernameTaken)
        {
            throw new ConflictException("EMAIL_OR_USERNAME_TAKEN", "Email or username already taken");
        }

        var user = new User(
            Guid.NewGuid(),
            request.Username,
            request.Email,
            _passwordHasher.Hash(request.Password));

        _users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.ToResponse(0, 0);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        User? user = await _users.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        AuthToken token = _tokenGenerator.GenerateToken(user);
        return new AuthResponse(
            token.Token,
            "Bearer",
            token.ExpiresInSeconds,
            new AuthUserDto(user.Id, user.Username));
    }
}
