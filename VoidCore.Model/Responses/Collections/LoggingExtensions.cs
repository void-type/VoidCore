namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// Helpers for pulling logging text from the default responses.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="itemSet"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string[] GetLogText<TEntity>(this IItemSet<TEntity> itemSet)
        {
            return new []
            {
                $"Count: {itemSet.Count}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="itemSetPage"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string[] GetLogText<TEntity>(this IItemSetPage<TEntity> itemSetPage)
        {
            return new []
            {
                $"Count: {itemSetPage.Count}",
                $"Page: {itemSetPage.Page}",
                $"Take: {itemSetPage.Take}",
                $"TotalCount: {itemSetPage.TotalCount}"
            };
        }
    }
}
