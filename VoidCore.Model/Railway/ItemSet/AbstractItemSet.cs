using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Railway.ItemSet
{
    /// <inheritdoc/>
    public abstract class AbstractItemSetBase<TEntity> : IItemSet<TEntity>
    {
        /// <inheritdoc/>
        public int Count => Items.Count();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Items { get; protected set; }
    }
}
