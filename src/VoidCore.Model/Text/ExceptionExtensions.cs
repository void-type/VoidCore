using System;
using System.Collections.Generic;

namespace VoidCore.Model.Text
{
    /// <summary>
    /// Extension methods to log exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Flatten nested exceptions to a set of strings.
        /// </summary>
        /// <param name="exception">The outer exception</param>
        public static IEnumerable<string> FlattenMessages(this Exception exception)
        {
            if (exception is null)
            {
                return Array.Empty<string>();
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
