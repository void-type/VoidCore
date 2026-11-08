using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Net;
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

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, post.StatusCode);
        Assert.DoesNotContain("SPA", await get.Content.ReadAsStringAsync(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Unmatched_page_requests_fall_back_to_the_spa()
    {
        using var client = await CreateClient();

        var response = await client.GetAsync(new Uri("/some/client/route", UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("SPA", await response.Content.ReadAsStringAsync());
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

    private static async Task<HttpClient> CreateClient()
    {
        var environment = Substitute.For<IHostEnvironment>();
        environment.EnvironmentName.Returns(Environments.Production);

        var host = await new HostBuilder()
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
[ApiRoute("empty")]
public class TestEmptyApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => HttpResponder.Respond(Result.Ok());
}

public class HomeController : Controller
{
    public IActionResult Index() => Content("SPA");
}
