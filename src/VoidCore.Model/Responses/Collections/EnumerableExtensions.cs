using System.Collections.Generic;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Extensions to make ItemSets from enumerable collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create an non-paged item set.
        /// </summary>
        /// <param name="items">The items to return in the set</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items)
        {
            return new ItemSet<T>(items);
        }

        /// <summary>
        /// Create an optionally paginated itemset. If pagination is enabled, it will be performed against the items.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="paginationOptions">Options to control pagination</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items, PaginationOptions paginationOptions)
        {
            return new ItemSet<T>(items, paginationOptions);
        }

        /// <summary>
        /// Create an item set from explicit properties. This extension bypasses pagination logic. This is useful if
        /// another service performed the pagination and the total count is known.
        /// </summary>
        /// <param name="items">The page from the total set of items</param>
        /// <param name="paginationOptions">Options to control pagination</param>
        /// <param name="totalCount">The count of the total set before pagination was performed</param>
        /// <typeparam name="T">The type of entities in the collection</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<T> ToItemSet<T>(this IEnumerable<T> items, PaginationOptions paginationOptions, int totalCount)
        {
            return new ItemSet<T>(items, paginationOptions, totalCount);
        }
    }
}
