using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace VoidCore.Domain
{
    /// <summary>
    /// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Transform the Maybe to a result whose success is dependent on the Maybe having a value.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="failure">The failure to put into the result if the Maybe has no value.</param>
        /// <typeparam name="T">The type of Maybe and Result value</typeparam>
        /// <returns>A result of the Maybe value</returns>
        public static IResult<T> ToResult<T>(this Maybe<T> maybe, IFailure failure)
        {
            return maybe.HasValue ?
                Result.Ok(maybe.Value) :
                Result.Fail<T>(failure);
        }

        /// <summary>
        /// Transform the Maybe to a result whose success is dependent on the Maybe having a value.
        /// </summary>
        /// <param name="maybeTask">A task to asynchronously retrieve the Maybe to transform</param>
        /// <param name="failure">The failure to put into the result if the Maybe has no value.</param>
        /// <typeparam name="T">The type of Maybe and Result value</typeparam>
        /// <returns>A result of the Maybe value</returns>
        public static async Task<IResult<T>> ToResultAsync<T>(this Task<Maybe<T>> maybeTask, IFailure failure)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.ToResult(failure);
        }

        /// <summary>
        /// Map the inner value to a Maybe of a new type by specifying the new value. The new value will be implicitly
        /// converted to a Maybe. If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="selector">The transforming map function</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static Maybe<TOut> Select<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, TOut> selector)
        {
            return maybe.HasValue ?
                selector(maybe.Value) :
                Maybe.None<TOut>();
        }

        /// <summary>
        /// Asynchronously map the inner value to a Maybe of a new type by specifying the new value. The new value will
        /// be implicitly converted to a Maybe. If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="selectorTask">An asynchronous task representing the transforming map function</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static async Task<Maybe<TOut>> SelectAsync<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<TOut>> selectorTask)
        {
            return maybe.HasValue ?
                await selectorTask(maybe.Value).ConfigureAwait(false) :
                Maybe.None<TOut>();
        }

        /// <summary>
        /// Asynchronously map the inner value to a Maybe of a new type by specifying the new value. The new value will
        /// be implicitly converted to a Maybe. If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to transform</param>
        /// <param name="selector">The transforming map function</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static async Task<Maybe<TOut>> SelectAsync<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, TOut> selector)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.Select(selector);
        }

        /// <summary>
        /// Asynchronously map the inner value to a Maybe of a new type by specifying the new value. The new value will
        /// be implicitly converted to a Maybe. If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to transform</param>
        /// <param name="selectorTask">An asynchronous task representing the transforming map function</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A Maybe of the new value</returns>
        public static async Task<Maybe<TOut>> SelectAsync<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, Task<TOut>> selectorTask)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return await maybe.SelectAsync(selectorTask).ConfigureAwait(false);
        }

        /// <summary>
        /// If the last maybe has a value, bind it into a new maybe without nesting Maybes.
        /// If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="selector">The transforming map function that returns a Maybe</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A new Maybe</returns>
        public static Maybe<TOut> Then<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Maybe<TOut>> selector)
        {
            return maybe.HasValue ?
                selector(maybe.Value) :
                Maybe.None<TOut>();
        }

        /// <summary>
        /// If the last maybe has a value, asynchronously bind it into a new maybe without nesting Maybes.
        /// If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybe">The Maybe to transform</param>
        /// <param name="selectorTask">
        /// An asynchronous task representing the transforming map function that returns a Maybe
        /// </param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A new Maybe</returns>
        public static async Task<Maybe<TOut>> ThenAsync<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> selectorTask)
        {
            return maybe.HasValue ?
                await selectorTask(maybe.Value).ConfigureAwait(false) :
                Maybe.None<TOut>();
        }

        /// <summary>
        /// If the last maybe has a value, asynchronously bind it into a new maybe without nesting Maybes.
        /// If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to transform</param>
        /// <param name="selector">The transforming map function that returns a Maybe</param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A new Maybe</returns>
        public static async Task<Maybe<TOut>> ThenAsync<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, Maybe<TOut>> selector)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.Then(selector);
        }

        /// <summary>
        /// If the last maybe has a value, asynchronously bind it into a new maybe without nesting Maybes.
        /// If the original maybe has no value, then Maybe.None is passed through.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to transform</param>
        /// <param name="selectorTask">
        /// An asynchronous task representing the transforming map function that returns a Maybe
        /// </param>
        /// <typeparam name="TIn">The type of the original value</typeparam>
        /// <typeparam name="TOut">The type of the new value</typeparam>
        /// <returns>A new Maybe</returns>
        public static async Task<Maybe<TOut>> ThenAsync<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, Task<Maybe<TOut>>> selectorTask)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return await maybe.ThenAsync(selectorTask).ConfigureAwait(false);
        }

        /// <summary>
        /// Safely extract the value from the Maybe. If there is no value in the Maybe, return the default value.
        /// </summary>
        /// <param name="maybe">The Maybe to get the value from</param>
        /// <param name="defaultValue">What to return if there isn't a value in the Maybe</param>
        /// <typeparam name="T">The type of value</typeparam>
        /// <returns>The value of the Maybe</returns>
        [return: MaybeNull]
        public static T Unwrap<T>(this Maybe<T> maybe, T? defaultValue = default)
        {
            return maybe.HasValue ?
                maybe.Value :
                defaultValue;
        }

        /// <summary>
        /// Safely extract the value from the Maybe. If there is no value in the Maybe, invoke a factory method to return
        /// the default value.
        /// </summary>
        /// <param name="maybe">The Maybe to get the value from</param>
        /// <param name="defaultValueFactory">A factory method to invoke if the Maybe doesn't have a value</param>
        /// <typeparam name="T">The type of value</typeparam>
        /// <returns>The value of the Maybe</returns>
        public static T Unwrap<T>(this Maybe<T> maybe, Func<T> defaultValueFactory)
        {
            return maybe.HasValue ?
                maybe.Value :
                defaultValueFactory();
        }

        /// <summary>
        /// Asynchronously and safely extract the value from the Maybe. If there is no value in the Maybe, return the
        /// default value.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to get the value from</param>
        /// <param name="defaultValue">What to return if there isn't a value in the Maybe</param>
        /// <typeparam name="T">The type of value</typeparam>
        /// <returns>The value of the Maybe</returns>
        public static async Task<T?> UnwrapAsync<T>(this Task<Maybe<T>> maybeTask, T? defaultValue = default)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.HasValue ?
                maybe.Value :
                defaultValue;
        }

        /// <summary>
        /// Asynchronously and safely extract the value from the Maybe. If there is no value in the Maybe, invoke a
        /// factory method to return the default value.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to get the value from</param>
        /// <param name="defaultValueFactory">A factory method to invoke if the Maybe doesn't have a value</param>
        /// <typeparam name="T">The type of value</typeparam>
        /// <returns>The value of the Maybe</returns>
        public static async Task<T> UnwrapAsync<T>(this Task<Maybe<T>> maybeTask, Func<T> defaultValueFactory)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.Unwrap(defaultValueFactory);
        }

        /// <summary>
        /// Filter the value using a predicate. If the value does not satisfy the predicate, a Maybe.None is returned.
        /// </summary>
        /// <param name="maybe">The Maybe to filter</param>
        /// <param name="predicate">The predicate used to filter the value</param>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns>A Maybe with value if the predicate is true</returns>
        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            return maybe.HasValue && predicate(maybe.Value) ?
                maybe :
                Maybe.None<T>();
        }

        /// <summary>
        /// Filter the value using a predicate. If the value does not satisfy the predicate, a Maybe.None is returned.
        /// </summary>
        /// <param name="maybeTask">An asynchronous task representing the Maybe to filter</param>
        /// <param name="predicate">The predicate used to filter the value</param>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <returns>A Maybe with value if the predicate is true</returns>
        public static async Task<Maybe<T>> WhereAsync<T>(this Task<Maybe<T>> maybeTask, Func<T, bool> predicate)
        {
            var maybe = await maybeTask.ConfigureAwait(false);

            return maybe.Where(predicate);
        }
    }
}
