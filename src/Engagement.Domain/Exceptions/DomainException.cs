namespace TikTokFeed.Engagement.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public abstract int StatusCode { get; }

    public abstract string Code { get; }

    protected DomainException(string message)
        : base(message)
    {
    }

    protected DomainException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
