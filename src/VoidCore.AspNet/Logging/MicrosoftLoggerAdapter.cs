using Microsoft.Extensions.Logging;
using System;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// An adapter for ILoggingService to use the AspNet loggerFactory supplied ILogger.
    /// </summary>
    public class MicrosoftLoggerAdapter : ILoggingService
    {
        private readonly ILoggingStrategy _eventLogger;
        private readonly ILogger _logger;

        /// <summary>
        /// Construct a new LoggerAdapter.
        /// </summary>
        /// <param name="loggerFactory">AspNet extensions logger factory</param>
        /// <param name="eventLogger">A strategy for logging an event to string</param>
        public MicrosoftLoggerAdapter(ILoggerFactory loggerFactory, ILoggingStrategy eventLogger)
        {
            _logger = loggerFactory.CreateLogger("Application");
            _eventLogger = eventLogger;
        }

        /// <inheritdoc/>
        public void Debug(Exception ex, params string[] messages)
        {
            _logger.LogDebug(_eventLogger.Log(ex, messages));
        }

        /// <inheritdoc/>
        public void Debug(params string[] messages)
        {
            _logger.LogDebug(_eventLogger.Log(messages));
        }

        /// <inheritdoc/>
        public void Error(Exception ex, params string[] messages)
        {
            _logger.LogError(_eventLogger.Log(ex, messages));
        }

        /// <inheritdoc/>
        public void Error(params string[] messages)
        {
            _logger.LogError(_eventLogger.Log(messages));
        }

        /// <inheritdoc/>
        public void Fatal(Exception ex, params string[] messages)
        {
            _logger.LogCritical(_eventLogger.Log(ex, messages));
        }

        /// <inheritdoc/>
        public void Fatal(params string[] messages)
        {
            _logger.LogCritical(_eventLogger.Log(messages));
        }

        /// <inheritdoc/>
        public void Info(Exception ex, params string[] messages)
        {
            _logger.LogInformation(_eventLogger.Log(ex, messages));
        }

        /// <inheritdoc/>
        public void Info(params string[] messages)
        {
            _logger.LogInformation(_eventLogger.Log(messages));
        }

        /// <inheritdoc/>
        public void Warn(Exception ex, params string[] messages)
        {
            _logger.LogWarning(_eventLogger.Log(ex, messages));
        }

        /// <inheritdoc/>
        public void Warn(params string[] messages)
        {
            _logger.LogWarning(_eventLogger.Log(messages));
        }
    }
}
