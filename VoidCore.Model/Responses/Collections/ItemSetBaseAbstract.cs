using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Responses.Collections
{
    /// <inheritdoc/>
    public abstract class ItemSetBaseAbstract<TEntity> : IItemSet<TEntity>
    {
        /// <inheritdoc/>
        public int Count => Items.Count();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Items { get; protected set; }
    }
}
