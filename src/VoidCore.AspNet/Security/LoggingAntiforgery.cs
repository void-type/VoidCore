using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VoidCore.AspNet.Logging;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Logs antiforgery validation failures as warnings. ASP.NET Core only logs them at Information level, which is
/// usually filtered out in production, leaving rejected requests (HTTP 400) with no explanation.
/// </summary>
internal sealed class LoggingAntiforgery : IAntiforgery
{
    private readonly IAntiforgery _inner;
    private readonly ILogger _logger;

    public LoggingAntiforgery(IAntiforgery inner, ILogger<LoggingAntiforgery> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
    {
        return _inner.GetAndStoreTokens(httpContext);
    }

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext)
    {
        return _inner.GetTokens(httpContext);
    }

    public async Task<bool> IsRequestValidAsync(HttpContext httpContext)
    {
        var isValid = await _inner.IsRequestValidAsync(httpContext).ConfigureAwait(false);

        if (!isValid)
        {
            AspNetLogMessages.AntiforgeryValidationFailed(_logger, httpContext.Request.Method, httpContext.Request.Path.Value, "The request was not valid.");
        }

        return isValid;
    }

    public async Task ValidateRequestAsync(HttpContext httpContext)
    {
        try
        {
            await _inner.ValidateRequestAsync(httpContext).ConfigureAwait(false);
        }
        catch (AntiforgeryValidationException exception)
        {
            AspNetLogMessages.AntiforgeryValidationFailed(_logger, httpContext.Request.Method, httpContext.Request.Path.Value, exception.Message);
            throw;
        }
    }

    public void SetCookieTokenAndHeader(HttpContext httpContext)
    {
        _inner.SetCookieTokenAndHeader(httpContext);
    }
}
