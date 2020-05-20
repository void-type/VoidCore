using System;
using System.Collections.Generic;

namespace VoidCore.Model.Text
{
    public static partial class TextHelpers
    {
        /// <summary>
        /// Flatten nested exceptions to a set of strings.
        /// </summary>
        /// <param name="exception">The outer exception</param>
        public static IEnumerable<string> FlattenExceptionMessages(Exception exception)
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
