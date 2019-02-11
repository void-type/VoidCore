using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Helpers for finalizing queries and transforming them into view DTOs.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Change an IEnumerable to a List then ItemSet.
        /// </summary>
        /// <param name="items">Collection of items</param>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>A new ItemSet of these items</returns>
        public static IItemSet<TEntity> ToItemSet<TEntity>(this IEnumerable<TEntity> items)
        {
            return new ItemSet<TEntity>(items);
        }

        /// <summary>
        /// Change an IEnumerable to a List then ItemSetPage.
        /// </summary>
        /// <param name="items">Collection of items</param>
        /// <param name="page">The page number to select of the set</param>
        /// <param name="take">The number of items per page</param>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>A new ItemSetPage of these items</returns>
        public static IItemSetPage<TEntity> ToItemSetPage<TEntity>(this IEnumerable<TEntity> items, int page, int take)
        {
            return new ItemSetPage<TEntity>(items, page, take);
        }
    }
}
