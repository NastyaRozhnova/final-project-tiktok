namespace TikTokFeed.Engagement.Domain.Exceptions;

public sealed class NotFoundException : DomainException
{
    public override int StatusCode => 404;

    public override string Code { get; }

    public NotFoundException(string message, string code = "NOT_FOUND")
        : base(message)
    {
        Code = code;
    }
}
