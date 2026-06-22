namespace TikTokFeed.Identity.Domain.Exceptions;

public sealed class InvalidCredentialsException : DomainException
{
    public override int StatusCode => 401;

    public override string Code => "INVALID_CREDENTIALS";

    public InvalidCredentialsException()
        : base("Invalid email or password")
    {
    }
}
