using System.Diagnostics.CodeAnalysis;

namespace VoidCore.Model.Functional
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
        [NotNull]
        T Value { get; }
    }
}
