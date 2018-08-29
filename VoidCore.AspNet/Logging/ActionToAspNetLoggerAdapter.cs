using Microsoft.Extensions.Logging;
using System;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// An adapter for ILoggingService to use the AspNet loggerFactory supplied ILogger.
    /// </summary>
    public class ActionToAspNetLoggerAdapter : ILoggingService
    {
        /// <summary>
        /// Construct a new LoggerAdapter.
        /// </summary>
        /// <param name="loggerFactory">AspNet extensions logger factory</param>
        /// <param name="eventLogger">A strategy for logging an event to string</param>
        public ActionToAspNetLoggerAdapter(ILoggerFactory loggerFactory, IEventLoggingStrategy eventLogger)
        {
            _logger = loggerFactory.CreateLogger("Application");
            _eventLogger = eventLogger;
        }

        /// <inheritdoc/>
        public void Debug(Exception ex, params string[] messages)
        {
            _logger.LogDebug(_eventLogger.LogEvent(ex, messages));
        }

        /// <inheritdoc/>
        public void Debug(params string[] messages)
        {
            _logger.LogDebug(_eventLogger.LogEvent(messages));
        }

        /// <inheritdoc/>
        public void Error(Exception ex, params string[] messages)
        {
            _logger.LogError(_eventLogger.LogEvent(ex, messages));
        }

        /// <inheritdoc/>
        public void Error(params string[] messages)
        {
            _logger.LogError(_eventLogger.LogEvent(messages));
        }

        /// <inheritdoc/>
        public void Fatal(Exception ex, params string[] messages)
        {
            _logger.LogCritical(_eventLogger.LogEvent(ex, messages));
        }

        /// <inheritdoc/>
        public void Fatal(params string[] messages)
        {
            _logger.LogCritical(_eventLogger.LogEvent(messages));
        }

        /// <inheritdoc/>
        public void Info(Exception ex, params string[] messages)
        {
            _logger.LogInformation(_eventLogger.LogEvent(ex, messages));
        }

        /// <inheritdoc/>
        public void Info(params string[] messages)
        {
            _logger.LogInformation(_eventLogger.LogEvent(messages));
        }

        /// <inheritdoc/>
        public void Warn(Exception ex, params string[] messages)
        {
            _logger.LogWarning(_eventLogger.LogEvent(ex, messages));
        }

        /// <inheritdoc/>
        public void Warn(params string[] messages)
        {
            _logger.LogWarning(_eventLogger.LogEvent(messages));
        }

        private readonly IEventLoggingStrategy _eventLogger;
        private readonly ILogger _logger;
    }
}
