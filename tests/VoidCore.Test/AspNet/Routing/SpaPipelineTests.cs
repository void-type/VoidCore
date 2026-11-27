using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.AspNet.Routing;

public class SpaPipelineTests
{
    [Fact]
    public async Task Unmatched_api_routes_return_404_instead_of_the_spa()
    {
        using var client = await CreateClient();

        var get = await client.GetAsync(new Uri("/api/typo", UriKind.Relative));
        var post = await client.PostAsync(new Uri("/api/typo", UriKind.Relative), new StringContent(string.Empty));
        var root = await client.GetAsync(new Uri("/api", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, post.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, root.StatusCode);
        Assert.DoesNotContain("SPA", await get.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Wrong_method_on_an_api_route_is_a_404_not_the_spa()
    {
        using var client = await CreateClient();

        var response = await client.PostAsync(new Uri("/api/empty", UriKind.Relative), new StringContent(string.Empty));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.DoesNotContain("SPA", await response.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/some/client/route")]
    [InlineData("/apiary")]
    [InlineData("/")]
    public async Task Unmatched_page_requests_fall_back_to_the_spa(string path)
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri(path, UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("SPA", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Api_exceptions_return_generic_problem_details_and_are_logged_in_production()
    {
        var logs = new CapturingLoggerProvider();
        using var client = await CreateClient(Environments.Production, logs);

        var response = await client.GetAsync(new Uri("/api/boom", UriKind.Relative));
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.DoesNotContain("secret detail", body, StringComparison.Ordinal);

        using var json = JsonDocument.Parse(body);
        Assert.Equal(500, json.RootElement.GetProperty("status").GetInt32());
        Assert.Equal("An error occurred while processing your request.", json.RootElement.GetProperty("title").GetString());
        Assert.False(string.IsNullOrEmpty(json.RootElement.GetProperty("traceId").GetString()));

        Assert.Contains(logs.Errors, e => e.Exception?.Message == "secret detail");
    }

    [Fact]
    public async Task Api_exceptions_include_the_exception_as_detail_in_development()
    {
        using var client = await CreateClient(Environments.Development);

        var response = await client.GetAsync(new Uri("/api/boom", UriKind.Relative));
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("secret detail", json.RootElement.GetProperty("detail").GetString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Page_exceptions_are_not_returned_as_problem_details()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/page-boom", UriKind.Relative));

        Assert.NotEqual("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("SPA", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Api_controllers_return_problem_details_for_requests_that_cant_be_bound()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/api/echo?page=abc", UriKind.Relative));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("page", await response.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Successful_empty_result_responds_with_an_empty_200()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/api/empty", UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Security_headers_are_not_duplicated_when_a_status_code_page_re_executes_the_pipeline()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/missing-file.png", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(["DENY"], response.Headers.GetValues("X-Frame-Options"));
        Assert.Equal(["same-origin"], response.Headers.GetValues("Cross-Origin-Opener-Policy"));
        Assert.Single(response.Headers.GetValues("Content-Security-Policy"));
    }

    [Fact]
    public async Task Reporting_endpoints_header_uses_unquoted_names_with_one_url_each()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/api/empty", UriKind.Relative));

        Assert.Equal(
            ["csp-endpoint=\"https://example.com/csp\", other.endpoint=\"https://example.com/other\""],
            response.Headers.GetValues("Reporting-Endpoints"));
    }

    [Theory]
    [InlineData("Upper")]
    [InlineData("has space")]
    [InlineData("")]
    public void Invalid_reporting_endpoint_names_are_rejected(string name)
    {
        var app = new ApplicationBuilder(new ServiceCollection().BuildServiceProvider());

        Assert.Throws<ArgumentException>(() =>
            app.UseReportingEndpoints(new Dictionary<string, string> { [name] = "https://example.com/csp" }));
    }

    [Theory]
    [InlineData("not a url")]
    [InlineData("/relative")]
    [InlineData("https://example.com/\"quoted")]
    public void Invalid_reporting_endpoint_urls_are_rejected(string url)
    {
        var app = new ApplicationBuilder(new ServiceCollection().BuildServiceProvider());

        Assert.Throws<ArgumentException>(() =>
            app.UseReportingEndpoints(new Dictionary<string, string> { ["csp-endpoint"] = url }));
    }

    private static async Task<HttpClient> CreateClient(string environmentName = "Production", ILoggerProvider? loggerProvider = null)
    {
        var environment = Substitute.For<IHostEnvironment>();
        environment.EnvironmentName.Returns(environmentName);

        var host = await new HostBuilder()
            .ConfigureLogging(logging =>
            {
                if (loggerProvider is not null)
                {
                    logging.AddProvider(loggerProvider);
                }
            })
            .ConfigureWebHost(webHost => webHost
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.AddControllers().AddApplicationPart(typeof(SpaPipelineTests).Assembly);
                    services.AddRouting();
                })
                .Configure(app =>
                {
                    app.UseSpaExceptionPage(environment);
                    app.UseRecommendedSecurityHeaders();
                    app.UseContentSecurityPolicy(csp => csp.DefaultSources.Allow("'self'"));
                    app.UseReportingEndpoints(new Dictionary<string, string>
                    {
                        ["csp-endpoint"] = "https://example.com/csp",
                        ["other.endpoint"] = "https://example.com/other",
                    });
                    app.UseRouting();
                    app.UseSpaEndpoints();
                }))
            .StartAsync();

        return host.GetTestClient();
    }
}

[ApiController]
[Route(ApiRoutes.BasePath + "/empty")]
public class TestEmptyApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => HttpResponder.Respond(Result.Ok());
}

[ApiController]
[Route(ApiRoutes.BasePath + "/boom")]
public class TestBoomApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => throw new InvalidOperationException("secret detail");
}

[ApiController]
[Route(ApiRoutes.BasePath + "/echo")]
public class TestEchoApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int page) => Ok(page);
}

public class HomeController : Controller
{
    public IActionResult Index() => Content("SPA");

    [HttpGet("page-boom")]
    public IActionResult Boom() => throw new InvalidOperationException("page secret");
}

internal sealed class CapturingLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentQueue<(LogLevel Level, Exception? Exception)> _entries = new();

    public IEnumerable<(LogLevel Level, Exception? Exception)> Errors => _entries.Where(e => e.Level == LogLevel.Error);

    public ILogger CreateLogger(string categoryName) => new CapturingLogger(_entries);

    public void Dispose() { }

    private sealed class CapturingLogger : ILogger
    {
        private readonly ConcurrentQueue<(LogLevel Level, Exception? Exception)> _entries;

        public CapturingLogger(ConcurrentQueue<(LogLevel Level, Exception? Exception)> entries) => _entries = entries;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _entries.Enqueue((logLevel, exception));
        }
    }
}
