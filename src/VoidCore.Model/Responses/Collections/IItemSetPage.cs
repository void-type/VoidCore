namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A page for paginating a set of results.
    /// </summary>
    public interface IItemSetPage<out T> : IItemSet<T>
    {
        /// <summary>
        /// The page number of this set.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// The requested number of results.
        /// </summary>
        int Take { get; }

        /// <summary>
        /// The count of the entire result set (total of all pages).
        /// </summary>
        int TotalCount { get; }
    }
}
