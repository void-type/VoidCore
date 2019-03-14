using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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
        public static IApplicationBuilder UseSpaExceptionPage(this IApplicationBuilder app, IHostingEnvironment environment)
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
        /// Add the spa fallback route. If any requests miss known endpoints, it will redirect to home/index.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseSpaMvcRoute(this IApplicationBuilder app)
        {
            return app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "spa-fallback",
                    template: "{*url}",
                    defaults : new { controller = "Home", action = "Index" });
            });
        }
    }
}
