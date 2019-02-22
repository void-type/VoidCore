using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A page of a full set of items. Used for data pagination.
    /// </summary>
    /// <typeparam name="T">The entity type of the set</typeparam>
    public class ItemSetPage<T> : ItemSetBaseAbstract<T>, IItemSetPage<T>
    {
        /// <inheritdoc/>
        public int Page { get; }

        /// <inheritdoc/>
        public int Take { get; }

        /// <inheritdoc/>
        public int TotalCount { get; }

        /// <summary>
        /// Create a page from an entire item set.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSetPage(IEnumerable<T> items, int page, int take)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Cannot make an ItemSetPage of null items.");
            }

            var itemsList = items.ToList();
            TotalCount = itemsList.Count();

            page = page > 1 ? page : 1;
            take = take > 0 ? take : 0;

            Page = page;
            Take = take;
            Items = itemsList
                .Skip((page - 1) * take)
                .Take(take);
        }

        /// <summary>
        /// Create an item set page from an already paginated set.
        /// </summary>
        /// <param name="pageItems">The page from set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="totalCount">The count of the whole set before it was paginated</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSetPage(IEnumerable<T> pageItems, int page, int take, int totalCount)
        {
            if (pageItems == null)
            {
                throw new ArgumentNullException(nameof(pageItems), "Cannot make an ItemSetPage of null items.");
            }

            Page = page;
            Take = take;
            TotalCount = totalCount;
            Items = pageItems.ToList();
        }
    }
}
