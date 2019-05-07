using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoidCore.Domain
{
    /// <summary>
    /// Extensions for the Result class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Combine many results to one result. If any have failed, this will return a new aggregate failed result. If
        /// none have failed, this will return a successful result.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <returns>A combined result</returns>
        public static IResult Combine(this IEnumerable<IResult> results)
        {
            return Result.Combine(results.ToArray());
        }

        /// <summary>
        /// Asynchronously combine many results to one result. If any have failed, this will return a new aggregate
        /// failed result. If none have failed, this will return a successful result.
        /// </summary>
        /// <param name="resultTasks">Asynchronous tasks representing the the results to combine</param>
        /// <returns>A combined result</returns>
        public static async Task<IResult> CombineAsync(this IEnumerable<Task<IResult>> resultTasks)
        {
            return await Result.CombineAsync(resultTasks.ToArray()).ConfigureAwait(false);
        }
    }
}
