using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Middleware for adding X-Frame-Options security headers to HTTP responses. Allows or denies this page from being
/// shown in an x-frame, i-frame, embed, or object tag.
/// </summary>
public class XFrameOptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly XFrameOptionsOptions _options;

    /// <summary>
    /// Construct a new XFrameOptionsMiddleware.
    /// </summary>
    /// <param name="next">The next RequestDelegate</param>
    /// <param name="options">The options for configuring the header.</param>
    public XFrameOptionsMiddleware(RequestDelegate next, XFrameOptionsOptions options)
    {
        _next = next;
        _options = options;
    }

    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    public Task Invoke(HttpContext context)
    {
        context.EnsureNotNull();

        var header = new XFrameOptionsHeader(_options);
        context.Response.Headers.Add(header.Key, header.Value);
        return _next(context);
    }
}
