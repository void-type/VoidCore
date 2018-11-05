using System.Collections.Generic;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// Common interface for results of operations that can fail or succeed with a value.
    /// </summary>
    /// <typeparam name="T">The value of the result</typeparam>
    public interface IResult<out T> : IResult
    {
        /// <summary>
        /// The value returned when successful.
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// Common interface for results of operations that can fail or succeed.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// A collection of failures if the operation was unsuccessful.
        /// </summary>
        IEnumerable<IFailure> Failures { get; }

        /// <summary>
        /// True if the operation failed.
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// True if the operation was successful.
        /// </summary>
        bool IsSuccess { get; }
    }
}
