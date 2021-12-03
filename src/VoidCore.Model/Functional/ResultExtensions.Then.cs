using System;
using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Result class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <returns>The new result</returns>
    public static IResult Then(this IResult result, Func<IResult> next)
    {
        return result.IsSuccess ?
            next() :
            Result.Fail(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static IResult<TOut> Then<TOut>(this IResult result, Func<IResult<TOut>> next)
    {
        return result.IsSuccess ?
            next() :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static IResult<TOut> Then<TIn, TOut>(this IResult<TIn> result, Func<TIn, IResult<TOut>> next)
    {
        return result.IsSuccess ?
            next(result.Value) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <returns>The new result</returns>
    public static IResult Then<TIn>(this IResult<TIn> result, Func<TIn, IResult> next)
    {
        return result.IsSuccess ?
            next(result.Value) :
            Result.Fail(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="nextTask">An asynchronous map function to transform input value to output value</param>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync(this IResult result, Func<Task<IResult>> nextTask)
    {
        return result.IsSuccess ?
            await nextTask().ConfigureAwait(false) :
            Result.Fail(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync(this Task<IResult> resultTask, Func<IResult> next)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Then(next);
    }

    /// <summary>
    /// If the last result was successful, bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="nextTask">An asynchronous map function to transform input value to output value</param>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync(this Task<IResult> resultTask, Func<Task<IResult>> nextTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.ThenAsync(nextTask).ConfigureAwait(false);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="nextTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TOut>(this IResult result, Func<Task<IResult<TOut>>> nextTask)
    {
        return result.IsSuccess ?
            await nextTask().ConfigureAwait(false) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TOut>(this Task<IResult> resultTask, Func<IResult<TOut>> next)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Then(next);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="nextTask">An asynchronous map function to transform input value to output value</param>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TOut>(this Task<IResult> resultTask, Func<Task<IResult<TOut>>> nextTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.ThenAsync(nextTask).ConfigureAwait(false);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="nextTask">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TIn, TOut>(this IResult<TIn> result, Func<TIn, Task<IResult<TOut>>> nextTask)
    {
        return result.IsSuccess ?
            await nextTask(result.Value).ConfigureAwait(false) :
            Result.Fail<TOut>(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask, Func<TIn, IResult<TOut>> next)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Then(next);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="nextTask">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <typeparam name="TOut">The value of the output result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult<TOut>> ThenAsync<TIn, TOut>(this Task<IResult<TIn>> resultTask, Func<TIn, Task<IResult<TOut>>> nextTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.ThenAsync(nextTask).ConfigureAwait(false);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="result">The result</param>
    /// <param name="nextTask">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync<TIn>(this IResult<TIn> result, Func<TIn, Task<IResult>> nextTask)
    {
        return result.IsSuccess ?
            await nextTask(result.Value).ConfigureAwait(false) :
            Result.Fail(result.Failures);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="next">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync<TIn>(this Task<IResult<TIn>> resultTask, Func<TIn, IResult> next)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.Then(next);
    }

    /// <summary>
    /// If the last result was successful, asynchronously bind it to another fallible function. Failures are passed through.
    /// </summary>
    /// <param name="resultTask">An asynchronous task representing the result</param>
    /// <param name="nextTask">The map function to transform input value to output value</param>
    /// <typeparam name="TIn">The value of the input result</typeparam>
    /// <returns>The new result</returns>
    public static async Task<IResult> ThenAsync<TIn>(this Task<IResult<TIn>> resultTask, Func<TIn, Task<IResult>> nextTask)
    {
        var result = await resultTask.ConfigureAwait(false);

        return await result.ThenAsync(nextTask).ConfigureAwait(false);
    }
}
