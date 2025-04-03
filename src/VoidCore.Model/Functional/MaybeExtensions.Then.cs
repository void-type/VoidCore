namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
{
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
}
