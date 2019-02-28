using System.Collections.Generic;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Helpers for finalizing queries and transforming them into view DTOs.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create a new ItemSet. Note that this will finalize deferred queries.
        /// </summary>
        /// <param name="items">The items to return in the set</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items)
        {
            return new ItemSet<T>(items);
        }

        /// <summary>
        /// Create a page from an entire item set.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items, int page, int take)
        {
            return new ItemSet<T>(items, page, take);
        }

        /// <summary>
        /// Create an item set from an already paginated set.
        /// </summary>
        /// <param name="pageItems">The page from set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="totalCount">The count of the whole set before it was paginated</param>
        /// <param name="isPagingEnabled">If paging is enabled in this set</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> pageItems, int page, int take, int totalCount, bool isPagingEnabled = true)
        {
            return new ItemSet<T>(pageItems, page, take, totalCount, isPagingEnabled);
        }
    }
}
