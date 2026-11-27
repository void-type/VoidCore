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
    /// <param name="includeDetails">Also include each check's description and exception message to every caller. These can expose server
    /// and database details (a failed SQL connection message, for example), so enable this only when the endpoint is reachable only by
    /// trusted monitors, for example by requiring authorization or a private host. To decide per request, use the overload that takes a function.</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder MapHealthEndpoint(this IEndpointRouteBuilder endpoints, string pattern = "/health", bool includeDetails = false)
    {
        return endpoints.MapHealthEndpoint(_ => includeDetails, pattern);
    }

    /// <summary>
    /// Maps the health check endpoint and decides for each request whether to include details. Returns 200 when healthy or degraded and
    /// 503 when unhealthy. The JSON body always includes the overall status and each check's name, status, and duration. Details are each
    /// check's description and exception message.
    /// For example, to show details to authenticated users only: <c>MapHealthEndpoint(context =&gt; context.User.Identity?.IsAuthenticated == true)</c>.
    /// </summary>
    /// <param name="endpoints">This endpoint route builder</param>
    /// <param name="includeDetails">A function that returns true to include details for the request</param>
    /// <param name="pattern">The route pattern of the endpoint</param>
    /// <returns>The endpoint convention builder for further configuration.</returns>
    public static IEndpointConventionBuilder MapHealthEndpoint(this IEndpointRouteBuilder endpoints, Func<HttpContext, bool> includeDetails, string pattern = "/health")
    {
        endpoints.EnsureNotNull();
        includeDetails.EnsureNotNull();

        return endpoints.MapHealthChecks(pattern, new()
        {
            ResponseWriter = (context, report) => WriteResponseAsync(context, report, includeDetails)
        });
    }

    /// <summary>
    /// Writes the health report as JSON with the overall status and the name, status, and duration of each check. Descriptions and
    /// exception messages are not included. Use the overloads with includeDetails to include them.
    /// </summary>
    /// <param name="context">The HTTP context to write to</param>
    /// <param name="report">The report to write</param>
    public static Task WriteResponseAsync(HttpContext context, HealthReport report)
    {
        return WriteResponseAsync(context, report, false);
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

    /// <summary>
    /// Writes the health report as JSON with the overall status and the result of each check. Details are included when the function
    /// returns true for the request.
    /// </summary>
    /// <param name="context">The HTTP context to write to</param>
    /// <param name="report">The report to write</param>
    /// <param name="includeDetails">A function that returns true to include each check's description and exception message</param>
    public static Task WriteResponseAsync(HttpContext context, HealthReport report, Func<HttpContext, bool> includeDetails)
    {
        context.EnsureNotNull();
        includeDetails.EnsureNotNull();

        return WriteResponseAsync(context, report, includeDetails(context));
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
