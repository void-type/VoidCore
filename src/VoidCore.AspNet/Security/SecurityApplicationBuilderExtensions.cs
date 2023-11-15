using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

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
    /// Set a Content Security Policy header on the HTTP response.
    /// See for directives and their uses: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
    /// Use https://csp-evaluator.withgoogle.com/ to evaluate your CSP.
    /// Denies unsafe content from being rendered on the page.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="builder">A callback to configure header options.</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app, Action<CspOptionsBuilder> builder)
    {
        builder.EnsureNotNull();

        return app.UseMiddleware<CspMiddleware>(builder);
    }

    // Remove obsolete code in next major version
#if NET8_0
#pragma warning disable S1133
#endif

    /// <summary>
    /// Set an X-Frame-Options header on the HTTP response. Allows or denies this page from being shown in an
    /// x-frame, i-frame, embed, or object tag. Eventually Content Security Policy's frame-ancestors will obsolete this.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="builder">A callback to configure header options.</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    [Obsolete("X-Frame-Options is an obsolete header. Use CSP's frame-ancestors instead.")]
    public static IApplicationBuilder UseXFrameOptions(this IApplicationBuilder app, Action<XFrameOptionsOptionsBuilder> builder)
    {
        builder.EnsureNotNull();

        var newBuilder = new XFrameOptionsOptionsBuilder();
        builder(newBuilder);
        var options = newBuilder.Build();

        return app.UseMiddleware<XFrameOptionsMiddleware>(options);
    }

#if NET8_0
#pragma warning restore S1133
#endif

    /// <summary>
    /// Set an X-Content-Type-Options header to nosniff on the HTTP response. Prevents browsers from sniffing MIME types and trying execute
    /// content that should not be executable. This can prevent XSS in some browsers from malicious user-uploaded files such as images.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseXContentTypeOptionsNoSniff(this IApplicationBuilder app)
    {
        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", new[] { "nosniff" });
            return next();
        });
    }
}
