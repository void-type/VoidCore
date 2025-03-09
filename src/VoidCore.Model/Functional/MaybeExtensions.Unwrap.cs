using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
{
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
}
