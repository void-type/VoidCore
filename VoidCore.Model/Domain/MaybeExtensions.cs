using System;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// Extensions for the Maybe class.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Map the inner value to a Maybe of a new type by specifying the new value. The new value will be implicitly converted to a Maybe.
        /// </summary>
        /// <param name="maybe">The Maybe</param>
        /// <param name="selector">The transforming selector function</param>
        /// <typeparam name="T">The type of the original value</typeparam>
        /// <typeparam name="TNew">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static Maybe<TNew> Select<T, TNew>(this Maybe<T> maybe, Func<T, TNew> selector)
        {
            return maybe.HasNoValue ?
                Maybe<TNew>.None :
                selector(maybe.Value);
        }

        /// <summary>
        /// Map the inner value to a Maybe of a new type by specifying the new Maybe.
        /// </summary>
        /// <param name="maybe">The Maybe</param>
        /// <param name="selector">The transforming selector function</param>
        /// <typeparam name="T">The type of the original value</typeparam>
        /// <typeparam name="TNew">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static Maybe<TNew> Select<T, TNew>(this Maybe<T> maybe, Func<T, Maybe<TNew>> selector)
        {
            return maybe.HasNoValue ?
                Maybe<TNew>.None :
                selector(maybe.Value);
        }

        /// <summary>
        /// Transform the Maybe to a result whose success is dependent on the Maybe having a value.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="errorMessage">The errorMessage of the result failure</param>
        /// <param name="uiHandle">The uiHandle of the result failure</param>
        /// <typeparam name="T">The type of Maybe and Result value</typeparam>
        /// <returns>A result of the Maybe having a value</returns>
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage, string uiHandle = null)
        {
            return maybe.HasNoValue ?
                Result.Fail<T>(errorMessage, uiHandle) :
                Result.Ok(maybe.Value);
        }

        /// <summary>
        /// Safely extract the value from the Maybe. If there is no value in the Maybe, this will return the defaultValue.
        /// </summary>
        /// <param name="maybe">The Maybe</param>
        /// <param name="defaultValue">What to return if there isn't a value in the Maybe</param>
        /// <typeparam name="T">The type of value</typeparam>
        /// <returns>The value of the Maybe</returns>
        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = default(T))
        {
            return maybe.Unwrap(x => x, defaultValue);
        }

        /// <summary>
        /// Safely extract and transform the value from the Maybe. If there is no value in the Maybe, this will return the defaultValue.
        /// </summary>
        /// <param name="maybe">The Maybe</param>
        /// <param name="selector">The transforming selector function</param>
        /// <param name="defaultValue">What to return if there isn't a value in the Maybe</param>
        /// <typeparam name="T">The type of the original value</typeparam>
        /// <typeparam name="TNew">The type of the new value</typeparam>
        /// <returns></returns>
        public static TNew Unwrap<T, TNew>(this Maybe<T> maybe, Func<T, TNew> selector, TNew defaultValue = default(TNew))
        {
            return maybe.HasValue ?
                selector(maybe.Value) :
                defaultValue;
        }

        /// <summary>
        /// Filter the value using a predicate. If the value does not satisfy the predicate, an empty Maybe is returned.
        /// </summary>
        /// <param name="maybe">The Maybe</param>
        /// <param name="predicate">The predicate used to filter the value</param>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns>A Maybe with value if the predicate is true</returns>
        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            return maybe.HasValue && predicate(maybe.Value) ?
                maybe :
                Maybe<T>.None;
        }
    }
}
