using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Domain
{
    /// <summary>
    /// Extensions for the Result class.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Combine many untyped results to one untyped result.
        /// If any have failed, this will return a new aggregate failed result. If none have failed, this will return
        /// a successful result.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <returns>A combined result</returns>
        public static IResult Combine(this IEnumerable<IResult> results)
        {
            return Result.Combine(results.ToArray());
        }

        /// <summary>
        /// Combine many typed results to an untyped result.
        /// If any have failed, this will return a new aggregate failed result. If none have failed, this will return
        /// a successful result.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <typeparam name="T">The type of value in the results</typeparam>
        /// <returns>A combined result</returns>
        public static IResult Combine<T>(this IEnumerable<IResult<T>> results)
        {
            return results.Select(result => (IResult) result).Combine();
        }
    }
}
