using System;
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

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static IResult TeeOnSuccess(this IResult result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnSuccess<T>(this IResult<T> result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next step in the pipeline.
        /// This side-effect takes the result value as a parameter.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnSuccess<T>(this IResult<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, perform a side-effect action then pass the original result through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static IResult TeeOnFailure(this IResult result, Action action)
        {
            if (result.IsFailed)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, perform a side-effect action then pass the original result through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnFailure<T>(this IResult<T> result, Action action)
        {
            if (result.IsFailed)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// Map the untyped result to a typed result. If the result is failed, the failures will be mapped to the new result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="selector">The map function to transform input value to output value</param>
        /// <typeparam name="TOutput">The value of the output result</typeparam>
        /// <returns>The new result</returns>
        public static IResult<TOutput> Select<TOutput>(this IResult result, Func<TOutput> selector)
        {
            return result.IsSuccess ?
                Result.Ok(selector()) :
                Result.Fail<TOutput>(result.Failures);
        }

        /// <summary>
        /// Map the result value to a result of a new type. If the result is failed, the failures will be mapped to the new result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="selector">The map function to transform input value to output value</param>
        /// <typeparam name="TInput">The value of the input result</typeparam>
        /// <typeparam name="TOutput">The value of the output result</typeparam>
        /// <returns>The new result</returns>
        public static IResult<TOutput> Select<TInput, TOutput>(this IResult<TInput> result, Func<TInput, TOutput> selector)
        {
            return result.IsSuccess ?
                Result.Ok(selector(result.Value)) :
                Result.Fail<TOutput>(result.Failures);
        }
    }
}
