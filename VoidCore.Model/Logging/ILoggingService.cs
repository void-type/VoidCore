using System;

namespace VoidCore.Model.Logging
{
    /// <summary>
    /// Common interface for logging within the model.
    /// </summary>
    public interface ILoggingService
    {

        /// <summary>
        /// Make a debug log message.
        /// </summary>
        /// <param name="messages">An array of messages to add to the log</param>
        void Debug(params string[] messages);

        /// <summary>
        /// Make a debug log message with exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="messages">An array of messages to add to the log</param>
        void Debug(Exception ex, params string[] messages);

        /// <summary>
        /// Make an error log message.
        /// </summary>
        /// <param name="messages">An array of messages to add to the log</param>
        void Error(params string[] messages);

        /// <summary>
        /// Make an error log message with exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="messages">An array of messages to add to the log</param>
        void Error(Exception ex, params string[] messages);

        /// <summary>
        /// Make a fatal log message.
        /// </summary>
        /// <param name="messages">An array of messages to add to the log</param>
        void Fatal(params string[] messages);

        /// <summary>
        /// Make a fatal log message with exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="messages">An array of messages to add to the log</param>
        void Fatal(Exception ex, params string[] messages);

        /// <summary>
        /// Make an informational log message.
        /// </summary>
        /// <param name="messages">An array of messages to add to the log</param>
        void Info(params string[] messages);

        /// <summary>
        /// Make an informational log message with exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="messages">An array of messages to add to the log</param>
        void Info(Exception ex, params string[] messages);

        /// <summary>
        /// Make a warning log message.
        /// </summary>
        /// <param name="messages">An array of messages to add to the log</param>
        void Warn(params string[] messages);

        /// <summary>
        /// Make a warning log message with exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="messages">An array of messages to add to the log</param>
        void Warn(Exception ex, params string[] messages);
    }
}
