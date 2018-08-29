namespace VoidCore.Model.Action.Responses.ItemSet
{
    /// <summary>
    /// A page for paginating a set of results.
    /// </summary>
    public interface IItemSetPage<out TEntity> : IItemSet<TEntity>
    {
        /// <summary>
        /// The page number of this set.
        /// </summary>
        /// <value></value>
        int Page { get; }

        /// <summary>
        /// The requested number of results.
        /// </summary>
        /// <value></value>
        int Take { get; }

        /// <summary>
        /// The count of the entire result set (total of all pages).
        /// </summary>
        /// <value></value>
        int TotalCount { get; }
    }
}
