using System;
using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
{
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
