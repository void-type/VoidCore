using System;
using System.Threading.Tasks;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extension methods for objects to support functional pipelines.
/// </summary>
public static partial class FunctionalExtensions
{
    /// <summary>
    /// Perform a side-effect action using the input within the action, then pass the input through to the next step
    /// in the pipeline.
    /// </summary>
    /// <param name="input">The input to the tee.</param>
    /// <param name="action">The action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static T Tee<T>(this T input, Action<T> action)
    {
        action.EnsureNotNull();

        action(input);

        return input;
    }

    /// <summary>
    /// Perform a side-effect action, then pass the input through to the next step in the pipeline.
    /// </summary>
    /// <param name="input">The input to the tee.</param>
    /// <param name="action">The action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static T Tee<T>(this T input, Action action)
    {
        action();

        return input;
    }

    /// <summary>
    /// Asynchronously perform a side-effect action using the input within the action, then pass the input through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="input">The input to the tee.</param>
    /// <param name="actionTask">The asynchronous action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this T input, Func<T, Task> actionTask)
    {
        actionTask.EnsureNotNull();

        await actionTask(input).ConfigureAwait(false);

        return input;
    }

    /// <summary>
    /// Asynchronously perform a side-effect action using the input within the action, then pass the input through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input to the tee</param>
    /// <param name="action">The action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this Task<T> inputTask, Action<T> action)
    {
        action.EnsureNotNull();

        var input = await inputTask.EnsureNotNull().ConfigureAwait(false);
        action(input);

        return input;
    }

    /// <summary>
    /// Asynchronously perform a side-effect action using the input within the action, then pass the input through to
    /// the next step in the pipeline.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input to the tee.</param>
    /// <param name="actionTask">The asynchronous action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this Task<T> inputTask, Func<T, Task> actionTask)
    {
        var input = await inputTask.ConfigureAwait(false);
        await actionTask(input).ConfigureAwait(false);

        return input;
    }

    /// <summary>
    /// Perform a side-effect action, then pass the input through to the next step in the pipeline.
    /// </summary>
    /// <param name="input">The input to the tee.</param>
    /// <param name="action">The action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this T input, Func<Task> action)
    {
        await action().ConfigureAwait(false);

        return input;
    }

    /// <summary>
    /// Asynchronously perform a side-effect action, then pass the input through to the next step in the pipeline.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input to the tee.</param>
    /// <param name="action">The action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this Task<T> inputTask, Action action)
    {
        var input = await inputTask.ConfigureAwait(false);
        action();

        return input;
    }

    /// <summary>
    /// Asynchronously perform a side-effect action, then pass the input through to the next step in the pipeline.
    /// </summary>
    /// <param name="inputTask">A task to asynchronously retrieve the input to the tee.</param>
    /// <param name="actionTask">The asynchronous action to perform.</param>
    /// <typeparam name="T">The type of input.</typeparam>
    public static async Task<T> TeeAsync<T>(this Task<T> inputTask, Func<Task> actionTask)
    {
        var input = await inputTask.ConfigureAwait(false);
        await actionTask().ConfigureAwait(false);

        return input;
    }
}
