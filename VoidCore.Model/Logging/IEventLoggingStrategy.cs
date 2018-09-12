using System;
using System.Collections.Generic;

namespace VoidCore.Model.Logging
{
    /// <summary>
    /// An interface for a strategy to log an event.
    /// </summary>
    public interface IEventLoggingStrategy
    {
        /// <summary>
        /// Transform an array of messages to a string.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        string LogEvent(IEnumerable<string> messages);

        /// <summary>
        /// Transform an array of messages and exception to a string.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        string LogEvent(Exception ex, IEnumerable<string> messages);

    }
}
