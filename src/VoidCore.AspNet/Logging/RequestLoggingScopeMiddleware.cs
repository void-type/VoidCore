using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Logging;

/// <summary>
/// Begin a logging scope for the current request that adds trace ID to all logging statements.
/// </summary>
public sealed class RequestLoggingScopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Construct a new RequestLoggingScopeMiddleware
    /// </summary>
    /// <param name="next">The next RequestDelegate</param>
    /// <param name="logger">A logger</param>
    public RequestLoggingScopeMiddleware(RequestDelegate next, ILogger<RequestLoggingScopeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    public async Task Invoke(HttpContext context)
    {
        context.EnsureNotNull();

        var traceId = context.TraceIdentifier;
        var request = context.Request;

        using var _ = _logger.BeginScope("{TraceId}:{RequestMethod}:{RequestPath}",
            traceId,
            request.Method,
            request.Path.Value);

        await _next(context);
    }
}
