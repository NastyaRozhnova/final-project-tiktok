var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8090");
var app = builder.Build();

int delayMs = int.TryParse(Environment.GetEnvironmentVariable("VISION_DELAY_MS"), out var d) ? d : 0;
double failRate = double.TryParse(Environment.GetEnvironmentVariable("VISION_FAIL_RATE"), out var f) ? f : 0.0;

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "vision-mock" }));

app.MapPost("/moderate", async (ModerateRequest req, ILogger<Program> log) =>
{
    if (delayMs > 0) await Task.Delay(delayMs);

    if (Random.Shared.NextDouble() < failRate)
    {
        log.LogWarning("Vision mock injecting failure for {VideoId}", req.video_id);
        return Results.StatusCode(503);
    }

    // Heuristic: reject empty captions or ones containing banned words.
    var caption = req.caption ?? string.Empty;
    var banned = new[] { "spam", "scam", "banned", "violation" };
    bool rejected = string.IsNullOrWhiteSpace(caption)
                    || banned.Any(w => caption.Contains(w, StringComparison.OrdinalIgnoreCase));

    var result = rejected
        ? new ModerateResponse("REJECTED", "Caption violates platform rules")
        : new ModerateResponse("APPROVED", null);

    log.LogInformation("Vision verdict for {VideoId}: {Status}", req.video_id, result.status);
    return Results.Ok(result);
});

app.Run();

record ModerateRequest(string video_id, string? video_url, string? caption);
record ModerateResponse(string status, string? reason);
