using VoidCore.Model.Guards;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extension methods for objects to support functional pipelines.
/// </summary>
public static partial class FunctionalExtensions
{
    /// <summary>
    /// Transform the input to the output.
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="selector">The map function to transform input to output</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    public static TOut Map<TIn, TOut>(this TIn input, Func<TIn, TOut> selector)
    {
        selector.EnsureNotNull();

        return selector(input);
    }

    /// <summary>
    /// Asynchronously transform the input to the output.
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="selectorTask">The asynchronous map function to transform input to output</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    public static Task<TOut> MapAsync<TIn, TOut>(this TIn input, Func<TIn, Task<TOut>> selectorTask)
    {
        selectorTask.EnsureNotNull();

        return selectorTask(input);
    }

    /// <summary>
    /// Asynchronously transform the input to the output.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input</param>
    /// <param name="selector">The map function to transform input to output</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> inputTask, Func<TIn, TOut> selector)
    {
        selector.EnsureNotNull();

        return selector(await inputTask.EnsureNotNull().ConfigureAwait(false));
    }

    /// <summary>
    /// Asynchronously transform the input to the output.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input</param>
    /// <param name="selectorTask">The asynchronous map function to transform input to output</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> inputTask, Func<TIn, Task<TOut>> selectorTask)
    {
        selectorTask.EnsureNotNull();

        return await selectorTask(await inputTask.EnsureNotNull().ConfigureAwait(false)).ConfigureAwait(false);
    }
}
