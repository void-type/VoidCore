using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A page of a full set of items. Used for data pagination.
    /// </summary>
    /// <typeparam name="TEntity">The entity type of the set</typeparam>
    public class ItemSetPage<TEntity> : ItemSetBaseAbstract<TEntity>, IItemSetPage<TEntity>
    {
        /// <inheritdoc/>
        public int Page { get; }

        /// <inheritdoc/>
        public int Take { get; }

        /// <inheritdoc/>
        public int TotalCount { get; }

        /// <summary>
        /// Create a new page of an item set. Note that this will finalize deferred queries. If running against Entity Framework, this will result in
        /// two database calls. However there is the benefit that pagination happens more efficiently on the database.
        /// </summary>
        /// <param name="items">The full set of items</param>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
        public ItemSetPage(IEnumerable<TEntity> items, int page, int take)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Cannot make an ItemSetPage of null items.");
            }

            var itemsArray = items.ToList();
            Items = SafePaginate(itemsArray, page, take);
            TotalCount = itemsArray.Count();
            Page = page;
            Take = take;
        }

        private const int PageFloor = 1;

        private const int TakeFloor = 0;

        private static int RangeFloor(int number, int lowerLimit)
        {
            return number < lowerLimit ? lowerLimit : number;
        }

        private static IEnumerable<TEntity> SafePaginate(IEnumerable<TEntity> items, int page, int take)
        {
            var safePage = RangeFloor(page, PageFloor);
            var safeTake = RangeFloor(take, TakeFloor);
            return items
                .Skip((safePage - 1) * safeTake)
                .Take(safeTake);
        }
    }
}
