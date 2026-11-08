using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using VoidCore.Model.Guards;

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
    /// always includes the overall status and each check's name, status, and duration.
    /// </summary>
    /// <param name="endpoints">This endpoint route builder</param>
    /// <param name="pattern">The route pattern of the endpoint</param>
    /// <param name="includeDetails">Also include each check's description and exception message. These can expose server and database
    /// details (a failed SQL connection message, for example), so enable this only when the endpoint is reachable only by trusted
    /// monitors, for example by requiring authorization or a private host.</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder MapHealthEndpoint(this IEndpointRouteBuilder endpoints, string pattern = "/health", bool includeDetails = false)
    {
        return endpoints.MapHealthChecks(pattern, new()
        {
            ResponseWriter = (context, report) => WriteResponseAsync(context, report, includeDetails)
        });
    }

    /// <summary>
    /// Writes the health report as JSON with the overall status and the result of each check, including each check's description and
    /// exception message.
    /// </summary>
    /// <param name="context">The HTTP context to write to</param>
    /// <param name="report">The report to write</param>
    public static Task WriteResponseAsync(HttpContext context, HealthReport report)
    {
        return WriteResponseAsync(context, report, true);
    }

    /// <summary>
    /// Writes the health report as JSON with the overall status and the result of each check.
    /// </summary>
    /// <param name="context">The HTTP context to write to</param>
    /// <param name="report">The report to write</param>
    /// <param name="includeDetails">Include each check's description and exception message</param>
    public static Task WriteResponseAsync(HttpContext context, HealthReport report, bool includeDetails)
    {
        context.EnsureNotNull();
        report.EnsureNotNull();

        var body = new
        {
            status = report.Status.ToString(),
            totalDurationMs = (long)report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(x => CreateCheck(x.Key, x.Value, includeDetails))
        };

        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body, _jsonOptions));
    }

    private static Dictionary<string, object?> CreateCheck(string name, HealthReportEntry entry, bool includeDetails)
    {
        var check = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["name"] = name,
            ["status"] = entry.Status.ToString(),
        };

        if (includeDetails)
        {
            check["description"] = entry.Description;
            check["error"] = entry.Exception?.Message;
        }

        check["durationMs"] = (long)entry.Duration.TotalMilliseconds;
        return check;
    }
}
