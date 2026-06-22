namespace TikTokFeed.Identity.Domain.Entities;

// Зарегистрированный пользователь платформы
public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string? ProfileInfo { get; private set; }

    public string? Avatar { get; private set; }

    public DateTime RegistrationDate { get; private set; }

    public bool IsCreator { get; private set; }

    public bool IsModerator { get; private set; }

    public User(Guid id, string username, string email, string passwordHash)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        RegistrationDate = DateTime.UtcNow;
    }

    private User()
    {
    }

    public void UpdateProfile(string? username, string? profileInfo, string? avatar)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            Username = username;
        }

        if (profileInfo is not null)
        {
            ProfileInfo = profileInfo;
        }

        if (avatar is not null)
        {
            Avatar = avatar;
        }
    }

    public void PromoteToCreator() => IsCreator = true;
}
