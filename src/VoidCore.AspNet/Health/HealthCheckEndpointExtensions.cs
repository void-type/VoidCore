using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace VoidCore.AspNet.Health;

/// <summary>
/// Maps a health check endpoint that reports failures in a JSON body so external monitors can relay them.
/// </summary>
public static class HealthCheckEndpointExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    /// <summary>
    /// Maps the health check endpoint. Returns 200 when healthy or degraded and 503 when unhealthy. The JSON body
    /// includes the description and exception message of each check, so only expose it to trusted monitors.
    /// </summary>
    /// <param name="endpoints">This endpoint route builder</param>
    /// <param name="pattern">The route pattern of the endpoint</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder MapHealthEndpoint(this IEndpointRouteBuilder endpoints, string pattern = "/health")
    {
        return endpoints.MapHealthChecks(pattern, new()
        {
            ResponseWriter = WriteResponseAsync
        });
    }

    /// <summary>
    /// Writes the health report as JSON with the overall status and the result of each check.
    /// </summary>
    /// <param name="context">The HTTP context to write to</param>
    /// <param name="report">The report to write</param>
    public static Task WriteResponseAsync(HttpContext context, HealthReport report)
    {
        var body = new
        {
            status = report.Status.ToString(),
            totalDurationMs = (long)report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                error = x.Value.Exception?.Message,
                durationMs = (long)x.Value.Duration.TotalMilliseconds
            })
        };

        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body, _jsonOptions));
    }
}
