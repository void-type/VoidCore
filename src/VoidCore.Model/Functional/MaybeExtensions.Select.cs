using System;
using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
{
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
}
