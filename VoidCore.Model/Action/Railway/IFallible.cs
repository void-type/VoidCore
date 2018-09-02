using System.Collections.Generic;

namespace VoidCore.Model.Action.Railway
{
    /// <summary>
    /// Common interface for results of operations that can fail or succeed.
    /// </summary>
    public interface IFallible
    {
        /// <summary>
        /// A collection of failures if the operation was unsuccessful.
        /// </summary>
        /// <value></value>
        IEnumerable<IFailure> Failures { get; }

        /// <summary>
        /// True if the operation was successful.
        /// </summary>
        /// <value></value>
        bool IsSuccess { get; }

        /// <summary>
        /// True if the operation failed.
        /// </summary>
        /// <value></value>
        bool IsFailed { get; }
    }
}
