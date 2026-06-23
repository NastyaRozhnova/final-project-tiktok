namespace TikTokFeed.Content.Domain.Exceptions;

public sealed class NotFoundException : DomainException
{
    public override int StatusCode => 404;

    public override string Code => "NOT_FOUND";

    public NotFoundException(string message)
        : base(message)
    {
    }
}
