var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8091");
var app = builder.Build();

double timeoutRate = double.TryParse(Environment.GetEnvironmentVariable("PUSH_TIMEOUT_RATE"), out var t) ? t : 0.0;
int delayMs = int.TryParse(Environment.GetEnvironmentVariable("PUSH_DELAY_MS"), out var d) ? d : 5000;

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "push-mock" }));

app.MapPost("/notify", async (NotifyRequest req, ILogger<Program> log) =>
{
    if (Random.Shared.NextDouble() < timeoutRate)
    {
        log.LogWarning("Push mock simulating timeout for {VideoId}", req.video_id);
        await Task.Delay(delayMs);
    }

    log.LogInformation("Notification sent: type={Type} video={VideoId} author={AuthorId}",
        req.type, req.video_id, req.author_id);
    return Results.Ok(new { delivered = true });
});

app.Run();

record NotifyRequest(string video_id, string author_id, string type);
