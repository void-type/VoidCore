using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Middleware for adding CSP security headers to HTTP responses.
/// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
/// </summary>
public sealed class CspMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Action<CspOptionsBuilder> _configure;
    private readonly NonceGenerator _nonceGenerator;

    /// <summary>
    /// Construct a new CspMiddleware.
    /// </summary>
    /// <param name="next">The next RequestDelegate</param>
    /// <param name="configure">An action to build options for configuring the header.</param>
    public CspMiddleware(RequestDelegate next, Action<CspOptionsBuilder> configure)
    {
        _next = next;
        _configure = configure;
        _nonceGenerator = new NonceGenerator();
    }

    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    public Task Invoke(HttpContext context)
    {
        context.EnsureNotNull();

        var nonce = _nonceGenerator.GetNonce();

        context.SetNonce(nonce);

        var builder = new CspOptionsBuilder(nonce);
        _configure(builder);
        var options = builder.Build();

        var header = new CspHeader(options);
        context.Response.Headers.Append(header.Key, header.Value);
        return _next(context);
    }
}
