using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Action.Responses.ItemSet
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
            Items = items.ToList();
        }
    }
}
