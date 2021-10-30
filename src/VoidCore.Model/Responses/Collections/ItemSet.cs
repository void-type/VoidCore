using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Guards;

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
        public ItemSet(IEnumerable<T> items) : this(items, PaginationOptions.None) { }

        /// <summary>
        /// Create an optionally paginated item set.
        /// </summary>
        /// <param name="items">The set of items. If paging is enabled and total count is not supplied, this set will be paged.</param>
        /// <param name="options">Options for pagination. Use PaginationOptions.None if pagination should not be performed.</param>
        /// <param name="totalCount">Optionally pass the count of the total set if pagination was already performed.</param>
        public ItemSet(IEnumerable<T> items, PaginationOptions options, int? totalCount = null)
        {
            items.EnsureNotNull(nameof(items));
            options.EnsureNotNull(nameof(options));

            var allItems = items.ToList();
            IsPagingEnabled = options.IsPagingEnabled;

            if (IsPagingEnabled)
            {
                TotalCount = totalCount ?? allItems.Count;
                Items = totalCount.HasValue ? allItems : allItems.GetPage(options);
                Page = options.Page;
                Take = options.Take;
            }
            else
            {
                Items = allItems;
                TotalCount = allItems.Count;
                Page = 1;
                Take = Math.Max(1, TotalCount);
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
