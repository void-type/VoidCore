using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VoidCore.Domain.Guards
{
    /// <summary>
    /// Static helpers to guard against invalid method arguments.
    /// </summary>
    public static class Guard
    {
        private const string _argumentNullMessage = "Argument cannot be null.";
        private const string _argumentEmptyMessage = "Argument cannot be empty.";
        private const string _argumentInvalidMessage = "Argument is invalid.";

        /// <summary>
        /// Ensure that the argument is not null.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="argumentName">
        /// The name of the argument. It is recommended to use nameof instead of hardcoding the parameter name.
        /// </param>
        /// <param name="message">An option to override the default exception message.</param>
        /// <typeparam name="T">The type of argument.</typeparam>
        /// <returns>The argument for chaining guards or assignment.</returns>
        [DebuggerStepThrough]
        public static T EnsureNotNull<T>(this T argumentValue, string argumentName, string message = null)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, message ?? _argumentNullMessage);
            }

            return argumentValue;
        }

        /// <summary>
        /// Ensure that the string argument is not null or empty.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="argumentName">
        /// The name of the argument. It is recommended to use nameof instead of hardcoding the parameter name.
        /// </param>
        /// <param name="message">An option to override the default exception message.</param>
        /// <returns>The argument for chaining guards or assignment.</returns>
        [DebuggerStepThrough]
        public static string EnsureNotNullOrEmpty(this string argumentValue, string argumentName, string message = null)
        {
            return argumentValue
                .EnsureNotNull(argumentName, message ?? _argumentNullMessage)
                .Ensure(v => !string.IsNullOrEmpty(v), argumentName, message ?? _argumentEmptyMessage);
        }

        /// <summary>
        /// Ensure the collection argument is not null or empty.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="argumentName">
        /// The name of the argument. It is recommended to use nameof instead of hardcoding the parameter name.
        /// </param>
        /// <param name="message">An option to override the default exception message.</param>
        /// <typeparam name="T">The type of argument.</typeparam>
        /// <returns>The argument for chaining guards or assignment.</returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> EnsureNotNullOrEmpty<T>(this IEnumerable<T> argumentValue, string argumentName, string message = null)
        {
            return argumentValue
                .EnsureNotNull(argumentName, message ?? _argumentNullMessage)
                .Ensure(v => v.Any(), argumentName, message ?? _argumentEmptyMessage);
        }

        /// <summary>
        /// Ensure the argument passes an arbitrary condition.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="conditionExpression">A function that if evaluates to false, will throw the exception.</param>
        /// <param name="argumentName">
        /// The name of the argument. It is recommended to use nameof instead of hardcoding the parameter name.
        /// </param>
        /// <param name="message">An option to override the default exception message.</param>
        /// <typeparam name="T">The type of argument.</typeparam>
        /// <returns>The argument for chaining guards or assignment.</returns>
        [DebuggerStepThrough]
        public static T Ensure<T>(this T argumentValue, Func<T, bool> conditionExpression, string argumentName, string message = null)
        {
            if (!conditionExpression(argumentValue))
            {
                throw new ArgumentException(message ?? _argumentInvalidMessage, argumentName);
            }

            return argumentValue;
        }

        /// <summary>
        /// Ensure the argument passes an arbitrary condition.
        /// </summary>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="conditionExpression">A function that if evaluates to false, will throw the exception.</param>
        /// <param name="argumentName">
        /// The name of the argument. It is recommended to use nameof instead of hardcoding the parameter name.
        /// </param>
        /// <param name="messageBuilder">An option to override the default exception message.</param>
        /// <typeparam name="T">The type of argument.</typeparam>
        /// <returns>The argument for chaining guards or assignment.</returns>
        [DebuggerStepThrough]
        public static T Ensure<T>(this T argumentValue, Func<T, bool> conditionExpression, string argumentName, Func<T, string> messageBuilder)
        {
            return argumentValue.Ensure(conditionExpression, argumentName, messageBuilder(argumentValue));
        }
    }
}
