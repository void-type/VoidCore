using System.Threading.Tasks;

namespace VoidCore.Model.Functional;

/// <summary>
/// Extensions for the Maybe class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public static partial class MaybeExtensions
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
}
