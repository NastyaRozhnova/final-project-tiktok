using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TikTokFeed.Engagement.IntegrationTests.Infrastructure;

internal static class TestJson
{
    public static StringContent Body(object value) =>
        new(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

    public static async Task<string?> ErrorCode(HttpResponseMessage response)
    {
        string json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        using JsonDocument doc = JsonDocument.Parse(json);
        return doc.RootElement.TryGetProperty("error", out JsonElement error)
               && error.TryGetProperty("code", out JsonElement code)
            ? code.GetString()
            : null;
    }

    public static async Task<JsonElement> Root(HttpResponseMessage response)
    {
        string json = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    public static bool IsNullOrMissing(JsonElement root, string name) =>
        !root.TryGetProperty(name, out JsonElement prop) || prop.ValueKind == JsonValueKind.Null;
}
