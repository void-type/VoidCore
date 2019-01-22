using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Configuration for security.
    /// </summary>
    public static class SecurityApplicationBuilderExtensions
    {
        /// <summary>
        /// Setup secure transport using HTTPS and HSTS. HSTS is disabled in development environments or when running on localhost, 127.0.0.1, or [::1].
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
    }
}