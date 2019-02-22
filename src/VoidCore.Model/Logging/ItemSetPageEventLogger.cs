using VoidCore.Model.Responses.Collections;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set page.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TEntity">The type of items in the item set</typeparam>
    public class ItemSetPageEventLogger<TRequest, TEntity> : FallibleEventLogger<TRequest, IItemSetPage<TEntity>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public ItemSetPageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, IItemSetPage<TEntity> response)
        {
            Logger.Info(
                $"Count: {response.Count}",
                $"Page: {response.Page}",
                $"Take: {response.Take}",
                $"TotalCount: {response.TotalCount}"
            );

            base.OnSuccess(request, response);
        }
    }
}
