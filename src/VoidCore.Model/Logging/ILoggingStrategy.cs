using System;
using System.Collections.Generic;

namespace VoidCore.Model.Logging
{
    /// <summary>
    /// An interface for a strategy to log an event. The strategy can transform and enrich the event.
    /// </summary>
    public interface ILoggingStrategy
    {
        /// <summary>
        /// Transform an array of messages to a string.
        /// </summary>
        /// <param name="messages">A collection of messages to log</param>
        /// <returns>The enriched and/or formatted log entry</returns>
        string Log(IEnumerable<string> messages);

        /// <summary>
        /// Transform an array of messages and exception to a string.
        /// </summary>
        /// <param name="ex">An exception to log</param>
        /// <param name="messages">A collection of messages to log</param>
        /// <returns>The enriched and/or formatted log entry</returns>
        string Log(Exception ex, IEnumerable<string> messages);
    }
}
