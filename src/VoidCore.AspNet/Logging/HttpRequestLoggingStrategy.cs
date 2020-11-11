using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// A strategy to log within HTTP Requests. This enriches log entries with information about the request such as
    /// current user name and request trace.
    /// </summary>
    public class HttpRequestLoggingStrategy : SimpleLoggingStrategy
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Construct a new strategy.
        /// </summary>
        /// <param name="httpContextAccessor">An accessor for the current HTTP context</param>
        /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
        public HttpRequestLoggingStrategy(IHttpContextAccessor httpContextAccessor, ICurrentUserAccessor currentUserAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <summary>
        /// Prepend the HTTP request method and path to concatenated messages. Messages are concatenated by spaces.
        /// </summary>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public override string Log(params string[] messages)
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return base.Log(messages);
            }

            var userName = _currentUserAccessor.User.Login;
            var request = _httpContextAccessor.HttpContext.Request;
            var traceId = _httpContextAccessor.HttpContext.TraceIdentifier;
            var prefix = $"{traceId}:{userName}:{request.Method}:{request.Path.Value}";

            return base.Log(messages.Prepend(prefix).ToArray());
        }

        /// <summary>
        /// Prepend the HTTP request method and path to concatenated messages and exception messages. Inner Exception
        /// messages are recursively flattened into a string.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public override string Log(Exception ex, params string[] messages)
        {
            return base.Log(ex, messages);
        }
    }
}
