using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <inheritdoc/>
    public class ItemSet<TEntity> : ItemSetBaseAbstract<TEntity>
    {
        /// <summary>
        /// Create a new ItemSet. Note that this will finalize deferred queries.
        /// </summary>
        /// <param name="items">The items to return in the set</param>
        /// <exception cref="System.ArgumentNullException">Throws an ArgumentNullException if null is passed for items.</exception>
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
