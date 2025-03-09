using System;
using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
{
    /// <summary>
    /// If the maybe has no value, perform a side-effect action then pass the original maybe through to the next step
    /// in the pipeline.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static Maybe<T> TeeOnFailure<T>(this Maybe<T> maybe, Action action)
    {
        if (maybe.HasNoValue)
        {
            action();
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has no value, asynchronously perform a side-effect action then pass the original maybe through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnFailureAsync<T>(this Maybe<T> maybe, Func<Task> actionTask)
    {
        if (maybe.HasNoValue)
        {
            await actionTask().ConfigureAwait(false);
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has no value, asynchronously perform a side-effect action then pass the original maybe through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnFailureAsync<T>(this Task<Maybe<T>> maybeTask, Action action)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return maybe.TeeOnFailure(action);
    }

    /// <summary>
    /// If the maybe has no value, asynchronously perform a side-effect action then pass the original maybe through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnFailureAsync<T>(this Task<Maybe<T>> maybeTask, Func<Task> actionTask)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return await maybe.TeeOnFailureAsync(actionTask).ConfigureAwait(false);
    }

    /// <summary>
    /// If the maybe has a value, perform a side-effect action then pass the original maybe through to the next
    /// step in the pipeline.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static Maybe<T> TeeOnSuccess<T>(this Maybe<T> maybe, Action action)
    {
        if (maybe.HasValue)
        {
            action();
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has a value, perform a side-effect action then pass the original maybe through to the next
    /// step in the pipeline. This side-effect takes the maybe value as a parameter.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="T">The value of the maybe</typeparam>
    /// <returns>The original maybe</returns>
    public static Maybe<T> TeeOnSuccess<T>(this Maybe<T> maybe, Action<T> action)
    {
        if (maybe.HasValue)
        {
            action(maybe.Value);
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Maybe<T> maybe, Func<Task> actionTask)
    {
        if (maybe.HasValue)
        {
            await actionTask().ConfigureAwait(false);
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="action">The action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Task<Maybe<T>> maybeTask, Action action)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return maybe.TeeOnSuccess(action);
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Task<Maybe<T>> maybeTask, Func<Task> actionTask)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return await maybe.TeeOnSuccessAsync(actionTask).ConfigureAwait(false);
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybe">The maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Maybe<T> maybe, Func<T, Task> actionTask)
    {
        if (maybe.HasValue)
        {
            await actionTask(maybe.Value).ConfigureAwait(false);
        }

        return maybe;
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="action">The action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Task<Maybe<T>> maybeTask, Action<T> action)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return maybe.TeeOnSuccess(action);
    }

    /// <summary>
    /// If the maybe has a value, asynchronously perform a side-effect action then pass the original maybe
    /// through to the next step in the pipeline.
    /// </summary>
    /// <param name="maybeTask">An asynchronous task representing the maybe</param>
    /// <param name="actionTask">The asynchronous action to perform</param>
    /// <returns>The original maybe</returns>
    public static async Task<Maybe<T>> TeeOnSuccessAsync<T>(this Task<Maybe<T>> maybeTask, Func<T, Task> actionTask)
    {
        var maybe = await maybeTask.ConfigureAwait(false);

        return await maybe.TeeOnSuccessAsync(actionTask).ConfigureAwait(false);
    }
}
