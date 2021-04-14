using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VoidCore.Model.Guards
{
    /// <summary>
    /// Static helpers to guard against invalid method arguments.
    /// </summary>
    public static class Guard
    {
        private const string ArgumentNullMessage = "Argument cannot be null.";
        private const string ArgumentEmptyMessage = "Argument cannot be empty.";
        private const string ArgumentInvalidMessage = "Argument is invalid.";

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
        /// <exception cref="ArgumentNullException">Throws when argumentValue null.</exception>
        [DebuggerStepThrough]
        [return: NotNull]
        public static T EnsureNotNull<T>(this T? argumentValue, string argumentName, string? message = null)
        {
            return argumentValue ?? throw new ArgumentNullException(argumentName, message ?? ArgumentNullMessage);
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
        [return: NotNull]
        public static string EnsureNotNullOrEmpty(this string? argumentValue, string argumentName, string? message = null)
        {
            return argumentValue
                .EnsureNotNull(argumentName, message ?? ArgumentNullMessage)
                .Ensure(v => !string.IsNullOrEmpty(v), argumentName, message ?? ArgumentEmptyMessage);
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
        [return: NotNull]
        public static IEnumerable<T> EnsureNotNullOrEmpty<T>(this IEnumerable<T>? argumentValue, string argumentName, string? message = null)
        {
            return argumentValue
                .EnsureNotNull(argumentName, message ?? ArgumentNullMessage)
                .Ensure(v => v.Any(), argumentName, message ?? ArgumentEmptyMessage);
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
        /// <exception cref="ArgumentException">Throws when condition expression evaluates false against argumentValue.</exception>
        [DebuggerStepThrough]
        public static T Ensure<T>(this T argumentValue, Func<T, bool> conditionExpression, string argumentName, string? message = null)
        {
            conditionExpression.EnsureNotNull(nameof(conditionExpression));

            return conditionExpression(argumentValue) ?
                argumentValue :
                throw new ArgumentException(message ?? ArgumentInvalidMessage, argumentName);
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
            messageBuilder.EnsureNotNull(nameof(messageBuilder));

            return argumentValue.Ensure(conditionExpression, argumentName, messageBuilder(argumentValue));
        }
    }
}
