using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// A strategy to log within HTTP Requests. This enriches the entries with information about the request such as
    /// current user name and request trace.
    /// </summary>
    public class HttpRequestLoggingStrategy : ILoggingStrategy
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
        /// Prepend the HTTP request method and path to concatenated messages. Messages are joined by spaces.
        /// </summary>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public string Log(params string[] messages)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var traceId = _httpContextAccessor.HttpContext.TraceIdentifier;
            var userName = _currentUserAccessor.User.Login;

            var prefix = $"{traceId}:{userName}:{request.Method}:{request.Path.Value}".PadRight(60);
            var payload = string.Join(" ", messages.Where(message => !string.IsNullOrWhiteSpace(message)));
            return string.Join(" ", prefix, payload);
        }

        /// <summary>
        /// Prepend the HTTP request method and path to concatenated messages and exception messages. Inner Exception
        /// messages are recursively flattened into a string.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public string Log(Exception ex, params string[] messages)
        {
            var eventArray = messages.Concat(FlattenExceptionMessages(ex)).ToArray();

            return Log(eventArray);
        }

        private static string[] FlattenExceptionMessages(Exception exception)
        {
            if (exception is null)
            {
                return new string[0];
            }

            var exceptionMessages = new List<string> { "Threw Exception:" };
            var stackTrace = exception.ToString();

            while (exception != null)
            {
                exceptionMessages.Add($"{exception.GetType()}: {exception.Message}");
                exception = exception.InnerException;
            }

            exceptionMessages.Add($"Stack Trace: {stackTrace}");
            return exceptionMessages.ToArray();
        }
    }
}
