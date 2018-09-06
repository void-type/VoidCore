using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Extensions to the IApplicationBuilder for setting up the web application pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add the spa fallback route. If any requests miss known endpoints, it will redirect to home/index.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder AddSpaMvcRoute(this IApplicationBuilder app)
        {
            return app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "spa-fallback",
                    template: "{*url}",
                    defaults : new { controller = "Home", action = "Index" });
            });
        }

        /// <summary>
        /// Setup secure transport using HTTPS and HSTS. HSTS is disabled in development environments.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <param name="environment">The hosting environment</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseSecureTransport(this IApplicationBuilder app, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            return app;
        }

        /// <summary>
        /// Setup exception pages for MVC view endpoints. Exceptions will redirect to /error. Forbidden requests will redirect to /forbidden. API
        /// endpoints will not redirect, they will return appropriate status codes. In development, all exceptions will return a debugging page. For
        /// API requests, you can see this page in the browser's developer console.
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
                    .StartsWithSegments(ApiRoute.BasePath, StringComparison.OrdinalIgnoreCase);

                if (isForbidden && !isApiRequest)
                {
                    response.Redirect("/forbidden");
                }
                return Task.FromResult<object>(null);
            });
            return app;
        }
    }
}
