using System.Text.Json.Serialization;

namespace TikTokFeed.Contracts.Errors;

public sealed class ErrorBody
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; init; } = string.Empty;
}
