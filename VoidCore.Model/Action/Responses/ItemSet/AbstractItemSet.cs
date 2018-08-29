using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Action.Responses.ItemSet
{
    /// <inheritdoc/>
    public abstract class AbstractItemSetBase<TEntity> : IItemSet<TEntity>
    {
        /// <inheritdoc/>
        public virtual int Count => Items?.Count() ?? 0;

        /// <inheritdoc/>
        public IEnumerable<TEntity> Items { get; protected set; }
    }
}
