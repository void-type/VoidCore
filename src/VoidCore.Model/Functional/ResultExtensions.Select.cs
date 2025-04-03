namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Result class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Map the untyped result to a typed result. If the result is failed, the failures will be mapped to the new result.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="selector">The map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static IResult<TOut> Select<TOut>(this IResult result, Func<TOut> selector)
    {
        return result.IsSuccess ?
            Result.Ok(selector()) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// Map the result value to a result of a new type. If the result is failed, the failures will be mapped to the
    /// new result.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="selector">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static IResult<TOut> Select<TIn, TOut>(this IResult<TIn> result, Func<TIn, TOut> selector)
    {
        return result.IsSuccess ?
            Result.Ok(selector(result.Value)) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// Asynchronously map the untyped result to a typed result. If the result is failed, the failures will be mapped
    /// to the new result.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="selectorTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TOut>(this IResult result, Func<Task<TOut>> selectorTask)
    {
        return result.IsSuccess ?
            Result.Ok(await selectorTask().ConfigureAwait(false)) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// Asynchronously map the untyped result to a typed result. If the result is failed, the failures will be mapped
    /// to the new result.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="selector">A map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TOut>(this Task<IResult> resultTask, Func<TOut> selector)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Select(selector);
    }

    /// <summary>
    /// Asynchronously map the untyped result to a typed result. If the result is failed, the failures will be mapped
    /// to the new result.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="selectorTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TOut>(this Task<IResult> resultTask, Func<Task<TOut>> selectorTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.SelectAsync(selectorTask).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously map the result value to a result of a new type. If the result is failed, the failures will be
    /// mapped to the new result.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="selectorTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TIn, TOut>(this IResult<TIn> result, Func<TIn, Task<TOut>> selectorTask)
    {
        return result.IsSuccess ?
            Result.Ok(await selectorTask(result.Value).ConfigureAwait(false)) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// Asynchronously map the result value to a result of a new type. If the result is failed, the failures will be
    /// mapped to the new result.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="selector">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask, Func<TIn, TOut> selector)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Select(selector);
    }

    /// <summary>
    /// Asynchronously map the result value to a result of a new type. If the result is failed, the failures will be
    /// mapped to the new result.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="selectorTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> SelectAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask, Func<TIn, Task<TOut>> selectorTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.SelectAsync(selectorTask).ConfigureAwait(false);
    }
}
