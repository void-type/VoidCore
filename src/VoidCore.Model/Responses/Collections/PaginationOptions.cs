namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Represents common options to control pagination.
    /// </summary>
    public class PaginationOptions
    {
        private PaginationOptions()
        {
        }

        /// <summary>
        /// Create a new pagination options.
        /// </summary>
        /// <param name="page">What page number to take from the set</param>
        /// <param name="take">How many items to include in each page</param>
        /// <param name="isPagingEnabled">If paging is enabled in this set. Default of pagination enabled</param>
        public PaginationOptions(int page, int take, bool isPagingEnabled = true)
        {
            Page = page;
            Take = take;
            IsPagingEnabled = isPagingEnabled;
        }

        /// <summary>
        /// Get a default pagination options that disables pagination.
        /// </summary>
        public static readonly PaginationOptions None = new PaginationOptions();

        /// <summary>
        /// How many items to include in each page.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// What page number to take from the set.
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// Should pagination be performed. If false, the whole set is included.
        /// </summary>
        public bool IsPagingEnabled { get; }
    }
}
