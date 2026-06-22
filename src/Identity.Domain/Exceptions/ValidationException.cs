namespace TikTokFeed.Identity.Domain.Exceptions;

public sealed class ValidationException : DomainException
{
    public override int StatusCode => 400;

    public override string Code => "VALIDATION_ERROR";

    public ValidationException(string message)
        : base(message)
    {
    }
}
