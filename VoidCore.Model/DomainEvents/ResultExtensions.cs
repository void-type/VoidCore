using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// Extensions for the Result class
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Combine many untyped results to one untyped result.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <returns>A combined result</returns>
        public static Result Combine(this IEnumerable<Result> results)
        {
            return Result.Combine(results.ToArray());
        }

        /// <summary>
        /// Combine many typed results to an untyped result.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <typeparam name="TValue">The type of value in the results</typeparam>
        /// <returns>A combined result</returns>
        public static Result Combine<TValue>(this IEnumerable<Result<TValue>> results)
        {
            return Result.Combine(results.ToArray());
        }
    }
}
