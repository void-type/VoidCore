using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace VoidCore.AspNet.Routing
{
    /// <summary>
    /// Configuration for SPA routing and exception handling.
    /// </summary>
    public static class RoutingApplicationBuilderExtensions
    {
        /// <summary>
        /// Setup exception pages for MVC view endpoints. Exceptions will redirect to /error. Forbidden requests will
        /// redirect to /forbidden. API endpoints will not redirect, they will return appropriate status codes. In
        /// development, all exceptions will return a debugging page. For API requests, you can see this page in the
        /// browser's developer console.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <param name="environment">The hosting environment</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseSpaExceptionPage(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;

                var isForbidden = response.StatusCode == StatusCodes.Status403Forbidden;

                var isApiRequest = context.HttpContext.Request.Path
                    .StartsWithSegments(ApiRouteAttribute.BasePath, StringComparison.OrdinalIgnoreCase);

                if (isForbidden && !isApiRequest)
                {
                    response.Redirect("/forbidden");
                }

                return Task.FromResult(0);
            });

            return app;
        }

        /// <summary>
        /// Map all SPA endpoint controllers and add a fallback route to "/" for unmatched requests.
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
    }
}
