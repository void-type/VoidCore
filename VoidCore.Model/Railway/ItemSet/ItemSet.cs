using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Railway.ItemSet
{
    /// <inheritdoc/>
    public class ItemSet<TEntity> : AbstractItemSetBase<TEntity>
    {
        /// <summary>
        /// Create a new ItemSet. Note that this will finalize deferred queries.
        /// </summary>
        /// <param name="items">The items to return in the set</param>
        public ItemSet(IEnumerable<TEntity> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Cannot make an ItemSet of null items.");
            }

            Items = items.ToList();
        }
    }
}
