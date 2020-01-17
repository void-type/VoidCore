using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Text;

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
            var eventArray = messages.Concat(TextHelpers.FlattenExceptionMessages(ex)).ToArray();

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
    }
}
