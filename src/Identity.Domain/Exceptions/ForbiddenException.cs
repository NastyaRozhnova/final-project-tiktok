namespace TikTokFeed.Identity.Domain.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public override int StatusCode => 403;

    public override string Code => "FORBIDDEN";

    public ForbiddenException(string message)
        : base(message)
    {
    }
}
