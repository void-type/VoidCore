using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Sets the Reporting-Endpoints header on the HTTP response with multiple endpoints.
    /// This header defines a set of named reporting endpoints that reports can be sent to.
    /// See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Reporting-Endpoints for more information.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="endpoints">Dictionary of endpoint names and their corresponding URLs</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseReportingEndpoints(this IApplicationBuilder app, Dictionary<string, string> endpoints)
    {
        endpoints.EnsureNotNull();

        return app.Use((context, next) =>
        {
            var reportingEndpoints = string.Join(", ",
                endpoints.Select(ep => $"\"{ep.Key}\"=\"{ep.Value}\""));

            context.Response.Headers.Append("Reporting-Endpoints", reportingEndpoints);
            return next();
        });
    }

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
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            return next();
        });
    }

    /// <summary>
    /// Sets the X-Frame-Options header to prevent clickjacking attacks by ensuring your content is not embedded into other sites via iframe.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="option">The X-Frame-Options value (DENY, SAMEORIGIN, or ALLOW-FROM)</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseXFrameOptions(this IApplicationBuilder app, string option = "SAMEORIGIN")
    {
        option.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("X-Frame-Options", option);
            return next();
        });
    }

    /// <summary>
    /// Sets the Permissions-Policy header to control which browser features and APIs can be used in the application.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="policy">The permissions policy directives</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UsePermissionsPolicy(this IApplicationBuilder app, string policy)
    {
        policy.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("Permissions-Policy", policy);
            return next();
        });
    }

    /// <summary>
    /// Sets the Referrer-Policy header to control how much referrer information should be included with requests.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="policy">The referrer policy (e.g., "no-referrer", "strict-origin-when-cross-origin")</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseReferrerPolicy(this IApplicationBuilder app, string policy = "strict-origin-when-cross-origin")
    {
        policy.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("Referrer-Policy", policy);
            return next();
        });
    }

    /// <summary>
    /// Sets the Cross-Origin-Embedder-Policy header to control resource loading.
    /// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cross-Origin-Embedder-Policy
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="policy">The policy value (e.g., "require-corp")</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseCrossOriginEmbedderPolicy(this IApplicationBuilder app, string policy = "require-corp")
    {
        policy.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("Cross-Origin-Embedder-Policy", policy);
            return next();
        });
    }

    /// <summary>
    /// Sets the Cross-Origin-Opener-Policy header to control window references.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="policy">The policy value (e.g., "same-origin")</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseCrossOriginOpenerPolicy(this IApplicationBuilder app, string policy = "same-origin")
    {
        policy.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("Cross-Origin-Opener-Policy", policy);
            return next();
        });
    }

    /// <summary>
    /// Sets the Cross-Origin-Resource-Policy header to prevent certain resources from being loaded by other origins.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <param name="policy">The policy value (e.g., "same-origin", "same-site", "cross-origin")</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseCrossOriginResourcePolicy(this IApplicationBuilder app, string policy = "same-origin")
    {
        policy.EnsureNotNull();

        return app.Use((context, next) =>
        {
            context.Response.Headers.Append("Cross-Origin-Resource-Policy", policy);
            return next();
        });
    }

    /// <summary>
    /// Applies all recommended security headers with fairly strict defaults.
    /// Includes: X-Content-Type-Options, X-Frame-Options, Referrer-Policy,
    /// Permissions-Policy, Cross-Origin-Embedder-Policy, Cross-Origin-Opener-Policy,
    /// and Cross-Origin-Resource-Policy. Be sure to also set Content-Security-Policy
    /// with UseContentSecurityPolicy.
    /// </summary>
    /// <param name="app">This IApplicationBuilder</param>
    /// <returns>The ApplicationBuilder for chaining.</returns>
    public static IApplicationBuilder UseRecommendedSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseXContentTypeOptionsNoSniff()
                 .UseXFrameOptions("DENY")
                 .UseReferrerPolicy("no-referrer")
                 .UsePermissionsPolicy("camera=(), microphone=(), geolocation=()")
                 .UseCrossOriginEmbedderPolicy()
                 .UseCrossOriginOpenerPolicy()
                 .UseCrossOriginResourcePolicy();
    }
}
