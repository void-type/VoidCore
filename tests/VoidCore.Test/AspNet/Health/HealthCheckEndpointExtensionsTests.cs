using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
    public async Task Write_response_includes_status_and_failure_messages_when_details_are_enabled()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await HealthCheckEndpointExtensions.WriteResponseAsync(context, CreateReport(), includeDetails: true);

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
    public async Task Write_response_hides_details_by_default()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await HealthCheckEndpointExtensions.WriteResponseAsync(context, CreateReport());

        context.Response.Body.Position = 0;
        using var json = await JsonDocument.ParseAsync(context.Response.Body);

        Assert.Equal("Unhealthy", json.RootElement.GetProperty("status").GetString());

        foreach (var check in json.RootElement.GetProperty("checks").EnumerateArray())
        {
            Assert.False(check.TryGetProperty("description", out _));
            Assert.False(check.TryGetProperty("error", out _));
        }
    }

    [Fact]
    public async Task Mapped_endpoint_decides_per_request_with_a_function()
    {
        using var client = await CreateClient(endpoints => endpoints.MapHealthEndpoint(context => context.Request.Headers.ContainsKey("X-Trusted")));

        var anonymous = await client.GetAsync(new Uri("/health", UriKind.Relative));

        using var trustedRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("/health", UriKind.Relative));
        trustedRequest.Headers.Add("X-Trusted", "1");
        var trusted = await client.SendAsync(trustedRequest);

        Assert.DoesNotContain("Server=secret", await anonymous.Content.ReadAsStringAsync(), StringComparison.Ordinal);
        Assert.Contains("Server=secret", await trusted.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Mapped_endpoint_with_a_function_uses_the_given_pattern()
    {
        using var client = await CreateClient(endpoints => endpoints.MapHealthEndpoint(_ => false, "/status"));

        var response = await client.GetAsync(new Uri("/status", UriKind.Relative));

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task Mapped_endpoint_hides_descriptions_and_error_messages_by_default()
    {
        using var client = await CreateClient(endpoints => endpoints.MapHealthEndpoint());

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
        using var client = await CreateClient(endpoints => endpoints.MapHealthEndpoint(includeDetails: true));

        var response = await client.GetAsync(new Uri("/health", UriKind.Relative));

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var check = json.RootElement.GetProperty("checks").EnumerateArray().Single();

        Assert.Equal("Index failed", check.GetProperty("description").GetString());
        Assert.Equal("Server=secret", check.GetProperty("error").GetString());
    }

    private static HealthReport CreateReport()
    {
        return new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["database"] = new(HealthStatus.Healthy, "Connected", TimeSpan.FromMilliseconds(5), null, null),
                ["search"] = new(HealthStatus.Unhealthy, "Index failed", TimeSpan.FromMilliseconds(7), new InvalidOperationException("Corrupt"), null)
            },
            TimeSpan.FromMilliseconds(12));
    }

    private static async Task<HttpClient> CreateClient(Action<IEndpointRouteBuilder> mapEndpoint)
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
                    .UseEndpoints(mapEndpoint)))
            .StartAsync();

        return host.GetTestClient();
    }
}
