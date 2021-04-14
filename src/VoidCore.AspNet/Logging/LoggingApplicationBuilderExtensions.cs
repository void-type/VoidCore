using Microsoft.AspNetCore.Builder;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// Configuration for logging.
    /// </summary>
    public static class LoggingApplicationBuilderExtensions
    {
        /// <summary>
        /// Begin a logging scope for the current request.
        /// </summary>
        /// <param name="app">The application builder</param>
        public static IApplicationBuilder UseRequestLoggingScope(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingScopeMiddleware>();
        }

        /// <summary>
        /// Log the current user's authorization.
        /// </summary>
        /// <param name="app">The application builder</param>
        public static IApplicationBuilder UseCurrentUserLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CurrentUserLoggingMiddleware>();
        }
    }
}
