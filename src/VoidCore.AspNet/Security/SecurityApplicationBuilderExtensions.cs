using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.Hosting;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Configuration for security.
    /// </summary>
    public static class SecurityApplicationBuilderExtensions
    {
        /// <summary>
        /// Setup secure transport using HTTPS and HSTS. HSTS is disabled in development environments or when running on
        /// localhost, 127.0.0.1, or [::1].
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <param name="environment">The hosting environment</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseSecureTransport(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                app.UseHsts();
            }

            return app.UseHttpsRedirection();
        }

        /// <summary>
        /// Set a Content Security Policy header on the HTTP response. See
        /// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy Denies unsafe content from
        /// being rendered on the page.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <param name="builder">A callback to configure header options.</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app, Action<CspOptionsBuilder> builder)
        {
            var newBuilder = new CspOptionsBuilder();
            builder(newBuilder);
            var options = newBuilder.Build();

            return app.UseMiddleware<CspMiddleware>(options);
        }

        /// <summary>
        /// Set an X-Frame-Options header on the HTTP response. Allows or denies this page from being shown in an
        /// x-frame, i-frame, embed, or object tag.
        /// </summary>
        /// <param name="app">This IApplicationBuilder</param>
        /// <param name="builder">A callback to configure header options.</param>
        /// <returns>The ApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseXFrameOptions(this IApplicationBuilder app, Action<XFrameOptionsOptionsBuilder> builder)
        {
            var newBuilder = new XFrameOptionsOptionsBuilder();
            builder(newBuilder);
            var options = newBuilder.Build();

            return app.UseMiddleware<XFrameOptionsMiddleware>(options);
        }
    }
}
