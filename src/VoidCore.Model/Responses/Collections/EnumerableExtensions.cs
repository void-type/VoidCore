using System.Collections.Generic;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Extensions to make ItemSets from enumerable collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create an item set without pagination.
        /// </summary>
        /// <param name="items">The items to return in the set</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items)
        {
            return new ItemSet<T>(items);
        }

        /// <summary>
        /// Create a paged item set with option to turn off pagination.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="isPagingEnabled">If paging is enabled in this set. Default of pagination enabled</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items, int page, int take, bool isPagingEnabled = true)
        {
            return new ItemSet<T>(items, page, take, isPagingEnabled);
        }

        /// <summary>
        /// Create an item set from explicit properties. This extension bypasses pagination logic.
        /// This is useful if another service performed the pagination and the total count is known.
        /// </summary>
        /// <param name="items">The page from set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="totalCount">The count of the whole set before it was paginated</param>
        /// <param name="isPagingEnabled">If paging is enabled in this set</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items, bool isPagingEnabled, int page, int take, int totalCount)
        {
            return new ItemSet<T>(items, isPagingEnabled, page, take, totalCount);
        }
    }
}
