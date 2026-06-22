namespace TikTokFeed.Identity.Domain.Exceptions;

// Нарушение бизнес-правила (409)
public class ConflictException : DomainException
{
    public override int StatusCode => 409;

    public override string Code { get; }

    public ConflictException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
