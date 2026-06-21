using System.Globalization;
using System.Text.Json.Serialization;

namespace TikTokFeed.Contracts.Errors;

public sealed class ErrorResponse
{
    [JsonPropertyName("error")]
    public ErrorBody Error { get; init; } = new();

    public static ErrorResponse Create(string code, string message) => new()
    {
        Error = new ErrorBody
        {
            Code = code,
            Message = message,
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
        },
    };
}
