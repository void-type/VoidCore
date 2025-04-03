using VoidCore.Model.Functional;

namespace VoidCore.Model.Responses.Collections;

/// <summary>
/// Extensions for IItemSet.
/// </summary>
public static class ItemSetExtensions
{
    /// <summary>
    /// Map the ItemSet items to a new type.
    /// </summary>
    /// <param name="itemSet">The ItemSet</param>
    /// <param name="selector">The map function to transform input item to output item</param>
    /// <typeparam name="TIn">The type of the input items</typeparam>
    /// <typeparam name="TOut">The type of the output items</typeparam>
    /// <returns>The new ItemSet</returns>
    public static IItemSet<TOut> Select<TIn, TOut>(this IItemSet<TIn> itemSet, Func<TIn, TOut> selector)
    {
        var paginationOptions = new PaginationOptions(itemSet.Page, itemSet.Take, itemSet.IsPagingEnabled);

        return itemSet
            .Items
            .Select(selector)
            .ToItemSet(paginationOptions, itemSet.TotalCount);
    }

    /// <summary>
    /// Asynchronously map a task returning ItemSet items to a new type.
    /// </summary>
    /// <param name="itemSetTask">A task returning the ItemSet</param>
    /// <param name="selector">The map function to transform input item to output item</param>
    /// <typeparam name="TIn">The type of the input items</typeparam>
    /// <typeparam name="TOut">The type of the output items</typeparam>
    /// <returns>The new ItemSet</returns>
    public static async Task<IItemSet<TOut>> SelectAsync<TIn, TOut>(this Task<IItemSet<TIn>> itemSetTask, Func<TIn, TOut> selector)
    {
        var itemSet = await itemSetTask;

        return itemSet.Select(selector);
    }
}
