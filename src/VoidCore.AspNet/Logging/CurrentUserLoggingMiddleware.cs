using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Logging;

/// <summary>
/// Log the current user's authorization.
/// </summary>
public partial class CurrentUserLoggingMiddleware
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
        context.EnsureNotNull();

        LogUser(_currentUserAccessor.User.Login, _currentUserAccessor.User.AuthorizedAs);

        return _next(context);
    }

    [LoggerMessage(0, LogLevel.Information, "User {Login} is authorized as {AuthorizedAs}.")]
    private partial void LogUser(string login, IEnumerable<string> authorizedAs);
}
