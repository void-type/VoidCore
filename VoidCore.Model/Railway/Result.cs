using System;
using System.Linq;

namespace VoidCore.Model.Railway
{
    /// <summary>
    /// The result of a faillible operation that returns a value on success.
    /// Generally used with CQRS Queries or other non-void fallible operations.
    /// Inspired by https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="TValue">The type of value to return on success</typeparam>
    public class Result<TValue> : ResultAbstract
    {
        /// <summary>
        /// The success value
        /// </summary>
        /// <value></value>
        public TValue Value { get; }

        internal Result(TValue value) : base(false, null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value),
                    "Cannot set a result of null. Use non-generic Result for void results.");
            }
            Value = value;
        }

        internal Result(IFailure[] failures) : base(true, failures) { }

        /// <summary>
        /// Implicitly convert a typed Result to an untyped one.
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result(Result<TValue> result)
        {
            return result.IsSuccess ? Result.Ok() : Result.Fail(result.Failures);
        }
    }

    /// <summary>
    /// The result of a faillible operation that does not return a value on success.
    /// Generally used with CQRS Commands or other void fallible operations.
    /// Inspired by https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public class Result : ResultAbstract
    {
        private Result() : base(false, null) { }

        private Result(IFailure[] failures) : base(true, failures) { }

        /// <summary>
        /// Create a new successful untyped result.
        /// </summary>
        /// <returns>A new result</returns>
        public static Result Ok()
        {
            return new Result();
        }

        /// <summary>
        /// Create a new successful typed result with a value.
        /// </summary>
        /// <param name="value">The result value</param>
        /// <typeparam name="TValue">The type of success value</typeparam>
        /// <returns>A new result</returns>
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>(value);
        }

        /// <summary>
        /// Create a new untyped failed result with a list a failures.
        /// </summary>
        /// <param name="failures">A list of failures</param>
        /// <returns>A new result</returns>
        public static Result Fail(IFailure[] failures)
        {
            return new Result(failures);
        }

        /// <summary>
        /// Create a new typed failed result with a list a failures.
        /// </summary>
        /// <param name="failures">A list of failures</param>
        /// <returns>A new result</returns>
        public static Result<TValue> Fail<TValue>(IFailure[] failures)
        {
            return new Result<TValue>(failures);
        }

        /// <summary>
        /// Create a new untyped result with a single failure.
        /// </summary>
        /// <param name="failure">The failure</param>
        /// <returns>A new result</returns>
        public static Result Fail(IFailure failure)
        {
            if (failure == null)
            {
                throw new ArgumentNullException(nameof(failure), "Failure must not be null for a failed result.");
            }

            return Fail(new [] { failure });
        }

        /// <summary>
        /// Create a new typed result with a single failure.
        /// </summary>
        /// <param name="failure">The failure</param>
        /// <returns>A new result</returns>
        public static Result<TValue> Fail<TValue>(IFailure failure)
        {
            if (failure == null)
            {
                throw new ArgumentNullException(nameof(failure), "Failure must not be null for a failed result.");
            }

            return Fail<TValue>(new [] { failure });
        }

        /// <summary>
        /// Create a new untyped result with a single failure using strings.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="uiHandle"></param>
        /// <returns>A new result</returns>
        public static Result Fail(string errorMessage, string uiHandle = null)
        {
            return Fail(new Failure(errorMessage, uiHandle));
        }

        /// <summary>
        /// Create a new typed result with a single failure using strings.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="uiHandle"></param>
        /// <returns>A new result</returns>
        public static Result<TValue> Fail<TValue>(string errorMessage, string uiHandle = null)
        {
            return Fail<TValue>(new Failure(errorMessage, uiHandle));
        }

        /// <summary>
        /// Combine several untyped results. If any have failed, this will return a new aggregate failed result.
        /// If none have failed, this will return a successful result.
        /// </summary>
        /// <param name="results">Results to combine</param>
        /// <returns>A new result</returns>
        public static Result Combine(params Result[] results)
        {
            var failures = results
                .Where(result => result.IsFailed)
                .SelectMany(result => result.Failures)
                .ToArray();

            return failures.Any() ? Fail(failures) : Ok();
        }

        /// <summary>
        /// Combine several typed results into an untyped one. If any have failed, this will return
        /// a new aggregate failed result. If none have failed, this will return an untyped successful result.
        /// </summary>
        /// <param name="results">Results to combine</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns>A new result</returns>
        public static Result Combine<TValue>(params Result<TValue>[] results)
        {
            var untypedResults = results
                .Select(result =>(Result) result)
                .ToArray();

            return Combine(untypedResults);
        }
    }
}
