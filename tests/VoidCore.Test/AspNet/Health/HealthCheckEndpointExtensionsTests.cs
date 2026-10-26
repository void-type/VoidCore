using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using VoidCore.AspNet.Health;
using Xunit;

namespace VoidCore.Test.AspNet.Health;

public class HealthCheckEndpointExtensionsTests
{
    [Fact]
    public async Task Write_response_includes_status_and_failure_messages()
    {
        var report = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["database"] = new(HealthStatus.Healthy, "Connected", TimeSpan.FromMilliseconds(5), null, null),
                ["search"] = new(HealthStatus.Unhealthy, "Index failed", TimeSpan.FromMilliseconds(7), new InvalidOperationException("Corrupt"), null)
            },
            TimeSpan.FromMilliseconds(12));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await HealthCheckEndpointExtensions.WriteResponseAsync(context, report);

        Assert.StartsWith("application/json", context.Response.ContentType);

        context.Response.Body.Position = 0;
        using var json = await JsonDocument.ParseAsync(context.Response.Body);
        var root = json.RootElement;

        Assert.Equal("Unhealthy", root.GetProperty("status").GetString());
        Assert.Equal(12, root.GetProperty("totalDurationMs").GetInt64());

        var checks = root.GetProperty("checks").EnumerateArray().ToList();
        Assert.Equal(2, checks.Count);
        Assert.Equal("Healthy", checks[0].GetProperty("status").GetString());
        Assert.Equal("Connected", checks[0].GetProperty("description").GetString());
        Assert.Equal(JsonValueKind.Null, checks[0].GetProperty("error").ValueKind);
        Assert.Equal("search", checks[1].GetProperty("name").GetString());
        Assert.Equal("Index failed", checks[1].GetProperty("description").GetString());
        Assert.Equal("Corrupt", checks[1].GetProperty("error").GetString());
    }
}
