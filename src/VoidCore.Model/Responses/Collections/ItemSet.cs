using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A set of items. Can be used for data pagination.
    /// </summary>
    /// <typeparam name="T">The entity type of the set</typeparam>
    public class ItemSet<T> : IItemSet<T>
    {
        /// <inheritdoc/>
        public IEnumerable<T> Items { get; }

        /// <inheritdoc/>
        public int Count => Items.Count();

        /// <inheritdoc/>
        public bool IsPagingEnabled { get; }

        /// <inheritdoc/>
        public int Page { get; }

        /// <inheritdoc/>
        public int Take { get; }

        /// <inheritdoc/>
        public int TotalCount { get; }

        /// <summary>
        /// Create a non-paged item set.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSet(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Cannot make an ItemSet of null items.");
            }

            var itemsList = items.ToList();
            TotalCount = itemsList.Count();
            Items = itemsList;
        }

        /// <summary>
        /// Create a page from an entire item set.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSet(IEnumerable<T> items, int page, int take)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Cannot make an ItemSet of null items.");
            }

            var allItems = items.ToList();

            IsPagingEnabled = true;
            Page = page;
            Take = take;
            TotalCount = allItems.Count();
            Items = allItems
                .Skip((page - 1) * take)
                .Take(take);
        }

        /// <summary>
        /// Create an item set from explicit properties. This constructor will not perform pagination.
        /// </summary>
        /// <param name="pageItems">The page from set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="totalCount">The count of the whole set before it was paginated</param>
        /// <param name="isPagingEnabled">Mark the set as paged or not paged</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSet(IEnumerable<T> pageItems, int page, int take, int totalCount, bool isPagingEnabled = true)
        {
            if (pageItems == null)
            {
                throw new ArgumentNullException(nameof(pageItems), "Cannot make an ItemSet of null items.");
            }

            IsPagingEnabled = isPagingEnabled;
            Page = page;
            Take = take;
            TotalCount = totalCount;
            Items = pageItems.ToList();
        }
    }
}
