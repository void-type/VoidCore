using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using VoidCore.Model.Text;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// Configuration for SPA routing and exception handling.
/// </summary>
public static class RoutingApplicationBuilderExtensions
{
    /// <summary>
    /// Setup exception pages for MVC view endpoints.
    /// Exceptions in development will show the Developer Exception Page.
    /// Exceptions in non-development will redirect to /error/500.
    /// Non-success status codes will redirect to /error/{statusCode}.
    /// Note that API endpoint exceptions are handled in the ApiRouteExceptionFilterAttribute. API status codes are returned verbatim, even if empty.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="environment">The hosting environment</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseSpaExceptionPage(this IApplicationBuilder app, IHostEnvironment environment)
    {
        // Handle uncaught page exceptions.
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error/500");
        }

        // Handle page non-success status codes with empty bodies. Ignore API requests.
        app.UseWhen(
            context => !ApiRouteAttribute.IsApiRequest(context),
            (appBuilder) => appBuilder.UseStatusCodePagesWithReExecute("/error/{0}")
        );

        return app;
    }

    /// <summary>
    /// Map all SPA endpoint controllers and add a fallback route to HomeController.Index for unmatched requests.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseSpaEndpoints(this IApplicationBuilder app)
    {
        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToController("Index", "Home");
        });
    }

    /// <summary>
    /// If AlwaysOn user agent is detected, we will return a simple response.
    /// </summary>
    /// <param name="app">The app for method chaining</param>
    /// <param name="userAgent">The UserAgent name to short circuit requests from.</param>
    public static IApplicationBuilder UseAlwaysOnShortCircuit(this IApplicationBuilder app, string userAgent = "AlwaysOn")
    {
        return app.Use(async (context, next) =>
        {
            if (context.Request.Headers.UserAgent.ToString().EqualsIgnoreCase(userAgent))
            {
                await context.Response.WriteAsync("Awake");
                return;
            }

            await next.Invoke();
        });
    }
}
