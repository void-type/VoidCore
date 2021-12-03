using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Logging;

/// <summary>
/// Log the current user's authorization.
/// </summary>
public sealed class CurrentUserLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILogger _logger;

    /// <summary>
    /// Construct a new CurrentUserLoggingMiddleware
    /// </summary>
    /// <param name="next">The next RequestDelegate</param>
    /// <param name="currentUserAccessor">Accessor for the current user</param>
    /// <param name="logger">A logger</param>
    public CurrentUserLoggingMiddleware(RequestDelegate next, ICurrentUserAccessor currentUserAccessor, ILogger<CurrentUserLoggingMiddleware> logger)
    {
        _next = next;
        _currentUserAccessor = currentUserAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context">The current HttpContext</param>
    public Task Invoke(HttpContext context)
    {
        context.EnsureNotNull(nameof(context));

        _logger.LogInformation("User {UserName} is authorized as {AuthorizedAs}.",
            _currentUserAccessor.User.Login,
            _currentUserAccessor.User.AuthorizedAs);

        return _next(context);
    }
}
