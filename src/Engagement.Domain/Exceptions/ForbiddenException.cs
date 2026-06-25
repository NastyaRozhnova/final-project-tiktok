namespace TikTokFeed.Engagement.Domain.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public override int StatusCode => 403;

    public override string Code { get; }

    public ForbiddenException(string message, string code = "FORBIDDEN")
        : base(message)
    {
        Code = code;
    }
}
