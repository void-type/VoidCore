using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Net;
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

    [Fact]
    public async Task Mapped_endpoint_hides_descriptions_and_error_messages_by_default()
    {
        using var client = await CreateClient(includeDetails: false);

        var response = await client.GetAsync(new Uri("/health", UriKind.Relative));
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.DoesNotContain("Server=secret", body, StringComparison.Ordinal);
        Assert.DoesNotContain("Index failed", body, StringComparison.Ordinal);

        using var json = JsonDocument.Parse(body);
        Assert.Equal("Unhealthy", json.RootElement.GetProperty("status").GetString());

        var check = json.RootElement.GetProperty("checks").EnumerateArray().Single();
        Assert.Equal("search", check.GetProperty("name").GetString());
        Assert.Equal("Unhealthy", check.GetProperty("status").GetString());
        Assert.False(check.TryGetProperty("description", out _));
        Assert.False(check.TryGetProperty("error", out _));
    }

    [Fact]
    public async Task Mapped_endpoint_includes_details_when_enabled()
    {
        using var client = await CreateClient(includeDetails: true);

        var response = await client.GetAsync(new Uri("/health", UriKind.Relative));

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var check = json.RootElement.GetProperty("checks").EnumerateArray().Single();

        Assert.Equal("Index failed", check.GetProperty("description").GetString());
        Assert.Equal("Server=secret", check.GetProperty("error").GetString());
    }

    private static async Task<HttpClient> CreateClient(bool includeDetails)
    {
        var host = await new HostBuilder()
            .ConfigureWebHost(webHost => webHost
                .UseTestServer()
                .ConfigureServices(services => services
                    .AddRouting()
                    .AddHealthChecks()
                    .AddCheck("search", () => HealthCheckResult.Unhealthy("Index failed", new InvalidOperationException("Server=secret"))))
                .Configure(app => app
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapHealthEndpoint(includeDetails: includeDetails))))
            .StartAsync();

        return host.GetTestClient();
    }
}
