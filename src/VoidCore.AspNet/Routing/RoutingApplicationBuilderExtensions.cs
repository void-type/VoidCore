using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using VoidCore.Model.Guards;
using VoidCore.Model.Text;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// Configuration for SPA routing and exception handling.
/// </summary>
public static class RoutingApplicationBuilderExtensions
{
    /// <summary>
    /// Setup exception handling for both MVC view endpoints and API endpoints.
    /// Page exceptions in development will show the Developer Exception Page. In non-development they redirect to /error/500, and
    /// non-success status codes redirect to /error/{statusCode}.
    /// Exceptions under /api are returned as a 500 <see cref="ProblemDetails"/> (application/problem+json) with a generic title and a trace
    /// id. In development the Detail also includes the exception. The exception is always logged by the framework. Other API status
    /// codes are returned verbatim, even if empty, unless the controller uses [ApiController], which adds ProblemDetails bodies.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="environment">The hosting environment</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseSpaExceptionPage(this IApplicationBuilder app, IHostEnvironment environment)
    {
        app.EnsureNotNull();
        environment.EnsureNotNull();

        // Handle uncaught API exceptions.
        app.UseWhen(
            ApiRoutes.IsApiRequest,
            apiBuilder => apiBuilder.UseExceptionHandler(errorBuilder => errorBuilder.Run(context => WriteApiProblemAsync(context, environment)))
        );

        // Handle uncaught page exceptions and page non-success status codes with empty bodies.
        app.UseWhen(
            context => !ApiRoutes.IsApiRequest(context),
            pageBuilder =>
            {
                if (environment.IsDevelopment())
                {
                    pageBuilder.UseDeveloperExceptionPage();
                }
                else
                {
                    pageBuilder.UseExceptionHandler("/error/500");
                }

                pageBuilder.UseStatusCodePagesWithReExecute("/error/{0}");
            }
        );

        return app;
    }

    private static Task WriteApiProblemAsync(HttpContext context, IHostEnvironment environment)
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "An error occurred while processing your request.",
            Detail = environment.IsDevelopment() ? exception?.ToString() : null,
        };

        problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json");
    }

    /// <summary>
    /// Map all SPA endpoint controllers. Requests under /api that don't match a controller aren't sent to the SPA. They get the
    /// framework's normal empty 404, including requests with the wrong method. Other unmatched requests fall back to
    /// HomeController.Index so client-side routes work.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseSpaEndpoints(this IApplicationBuilder app)
    {
        // The SPA fallback would otherwise answer unmatched API requests with HTML. Dropping the endpoint lets them 404. The fallback
        // is the only Home.Index endpoint without an attribute route.
        app.Use((context, next) =>
        {
            if (ApiRoutes.IsApiRequest(context) && IsSpaFallback(context.GetEndpoint()))
            {
                context.SetEndpoint(null);
            }

            return next(context);
        });

        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToController("Index", "Home");
        });
    }

    private static bool IsSpaFallback(Endpoint? endpoint)
    {
        return endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>() is { AttributeRouteInfo: null } descriptor
            && string.Equals(descriptor.ControllerName, "Home", StringComparison.Ordinal)
            && string.Equals(descriptor.ActionName, "Index", StringComparison.Ordinal);
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
