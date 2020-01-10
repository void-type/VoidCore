using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Logging
{
    /// <summary>
    /// A simple logging strategy.
    /// </summary>
    public class SimpleLoggingStrategy : ILoggingStrategy
    {
        /// <summary>
        /// Messages are concatenated by spaces.
        /// </summary>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public virtual string Log(params string[] messages)
        {
            return BuildPayload(messages);
        }

        /// <summary>
        /// Inner Exception messages are recursively flattened and appended to the concatenated messages.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="messages">Array of messages to log</param>
        /// <returns>The enriched log entry</returns>
        public virtual string Log(Exception ex, params string[] messages)
        {
            var eventArray = messages.Concat(FlattenExceptionMessages(ex)).ToArray();

            return Log(eventArray);
        }

        /// <summary>
        /// Format and join individual messages to a single string.
        /// </summary>
        /// <param name="messages">The messages to format</param>
        protected string BuildPayload(IEnumerable<string> messages)
        {
            return string.Join(" ", messages.Where(message => !string.IsNullOrWhiteSpace(message)));
        }

        /// <summary>
        /// Flatten nested exceptions to a set of strings.
        /// </summary>
        /// <param name="exception">The outer exception</param>
        protected static IEnumerable<string> FlattenExceptionMessages(Exception exception)
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
            return exceptionMessages;
        }
    }
}
