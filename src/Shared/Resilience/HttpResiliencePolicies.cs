using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace TikTokFeed.Contracts.Resilience;

public static class HttpResiliencePolicies
{
    private const int DefaultRetries = 3;

    public static IAsyncPolicy<HttpResponseMessage> Timeout(double seconds = 2) =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds), TimeoutStrategy.Optimistic);

    public static IAsyncPolicy<HttpResponseMessage> Retry(ILogger? logger = null, int retries = DefaultRetries) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retries,
                attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt - 1)),
                (outcome, delay, attempt, _) =>
                    logger?.LogWarning(
                        "Retry {Attempt} after {Delay}ms: {Reason}",
                        attempt,
                        delay.TotalMilliseconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()));

    public static IAsyncPolicy<HttpResponseMessage> CircuitBreaker(ILogger? logger = null) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(15),
                onBreak: (outcome, breakDelay) =>
                    logger?.LogError(
                        "Circuit OPEN for {Delay}s: {Reason}",
                        breakDelay.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()),
                onReset: () => logger?.LogInformation("Circuit reset (closed)"));
}
