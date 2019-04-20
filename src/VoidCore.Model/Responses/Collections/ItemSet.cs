using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain.Guards;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A set of items. Can be used for data pagination.
    /// </summary>
    /// <typeparam name="T">The entity type of the set</typeparam>
    public class ItemSet<T> : IItemSet<T>
    {
        /// <summary>
        /// Create an non-paged item set.
        /// </summary>
        /// <param name="items">The full set of items</param>
        public ItemSet(IEnumerable<T> items) : this(items, 0, 0, false) { }

        /// <summary>
        /// Create an optionally paginated itemset.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="isPagingEnabled">If paging is enabled in this set. Default of pagination enabled</param>
        public ItemSet(IEnumerable<T> items, int page, int take, bool isPagingEnabled = true)
        {
            items.EnsureNotNull(nameof(items));

            var allItems = items.ToList();
            IsPagingEnabled = isPagingEnabled;
            TotalCount = allItems.Count;

            if (isPagingEnabled)
            {
                Page = page;
                Take = take;
                Items = allItems
                    .Skip((page - 1) * take)
                    .Take(take);
            }
            else
            {
                Page = 1;
                Take = TotalCount > 1 ? TotalCount : 1;
                Items = allItems;
            }
        }

        /// <summary>
        /// Create an item set from explicit properties. This extension bypasses pagination logic. This is useful if
        /// another service performed the pagination and the total count is known.
        /// </summary>
        /// <param name="items">The page from set of items</param>
        /// <param name="isPagingEnabled">Mark the set as paged or not paged</param>
        /// <param name="page">What page number to take from the set. Ignored if pagination is disabled</param>
        /// <param name="take">How many items to include in each page. Ignored if pagination is disabled</param>
        /// <param name="totalCount">The total count. Ignored if pagination is disabled</param>
        public ItemSet(IEnumerable<T> items, int page, int take, int totalCount, bool isPagingEnabled = true)
        {
            items.EnsureNotNull(nameof(items));

            var allItems = items.ToList();
            IsPagingEnabled = isPagingEnabled;
            Items = allItems;

            if (isPagingEnabled)
            {
                TotalCount = totalCount;
                Page = page;
                Take = take;
            }
            else
            {
                TotalCount = allItems.Count;
                Page = 1;
                Take = TotalCount > 1 ? TotalCount : 1;
            }
        }

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
    }
}
