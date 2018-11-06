using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// The result of a fallible operation that returns a value on success. Generally used with CQRS Queries or other non-void fallible operations.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="T">The type of value to return on success</typeparam>
    public sealed class Result<T> : ResultAbstract, IResult<T>
    {
        /// <summary>
        /// The success value
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Throws an InvalidOperationException if accessed on a failed Result.</exception>
        public T Value
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Do not access the value of Result if it is failed.");
                }

                return _value;
            }
        }

        /// <summary>
        /// Implicitly convert a typed Result to an untyped one.
        /// </summary>
        /// <param name="result">The result to convert</param>
        public static implicit operator Result(Result<T> result)
        {
            return result.IsSuccess ? Result.Ok() : Result.Fail(result.Failures);
        }

        internal Result(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value),
                    "Cannot set a result value of null. Use non-generic Result for void results.");
            }

            _value = value;
        }

        internal Result(IEnumerable<IFailure> failures) : base(failures) { }

        private T _value;
    }

    /// <summary>
    /// The result of a fallible operation that does not return a value on success.
    /// Generally used with CQRS Commands or other void fallible operations.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public sealed class Result : ResultAbstract
    {
        /// <summary>
        /// Combine several untyped results. If any have failed, this will return a new aggregate failed result. If none have failed, this will return
        /// a successful result.
        /// </summary>
        /// <param name="results">Results to combine</param>
        /// <returns>A new result</returns>
        public static Result Combine(params IResult[] results)
        {
            var failures = results
                .Where(result => result.IsFailed)
                .SelectMany(result => result.Failures)
                .ToArray();

            return failures.Any() ? Fail(failures) : Ok();
        }

        /// <summary>
        /// Create a new untyped failed result with a list a failures.
        /// </summary>
        /// <param name="failures">A list of failures</param>
        /// <returns>A new result</returns>
        public static Result Fail(IEnumerable<IFailure> failures)
        {
            return new Result(failures);
        }

        /// <summary>
        /// Create a new typed failed result with a list a failures.
        /// </summary>
        /// <param name="failures">A list of failures</param>
        /// <returns>A new result</returns>
        public static Result<T> Fail<T>(IEnumerable<IFailure> failures)
        {
            return new Result<T>(failures);
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
        public static Result<T> Fail<T>(IFailure failure)
        {
            if (failure == null)
            {
                throw new ArgumentNullException(nameof(failure), "Failure must not be null for a failed result.");
            }

            return Fail<T>(new [] { failure });
        }

        /// <summary>
        /// Create a new untyped result with a single failure using strings.
        /// </summary>
        /// <param name="errorMessage">The error message to put into the failed result</param>
        /// <param name="uiHandle">The uiHandle to put into the failed result</param>
        /// <returns>A new result</returns>
        public static Result Fail(string errorMessage, string uiHandle = null)
        {
            return Fail(new Failure(errorMessage, uiHandle));
        }

        /// <summary>
        /// Create a new typed result with a single failure using strings.
        /// </summary>
        /// <param name="errorMessage">The error message to put into the failed result</param>
        /// <param name="uiHandle">The uiHandle to put into the failed result</param>
        /// <returns>A new result</returns>
        public static Result<T> Fail<T>(string errorMessage, string uiHandle = null)
        {
            return Fail<T>(new Failure(errorMessage, uiHandle));
        }

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
        /// <typeparam name="T">The type of success value</typeparam>
        /// <returns>A new result</returns>
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value);
        }

        private Result() { }

        private Result(IEnumerable<IFailure> failures) : base(failures) { }
    }
}
