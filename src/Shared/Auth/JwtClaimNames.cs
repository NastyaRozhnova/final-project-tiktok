namespace TikTokFeed.Contracts.Auth;

public static class JwtClaimNames
{
    public static string Subject => "sub";

    public static string Username => "unique_name";

    public static string Role => "role";

    public static string RoleModerator => "moderator";
}
