using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses.Collections
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TEntity">The type of items in the item set</typeparam>
    public class ItemSetEventLogger<TRequest, TEntity> : FallibleEventLogger<TRequest, IItemSet<TEntity>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public ItemSetEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, IItemSet<TEntity> response)
        {
            Logger.Info(
                $"Count: {response.Count}",
                $"IsPagingEnabled: {response.IsPagingEnabled}",
                $"Page: {response.Page}",
                $"Take: {response.Take}",
                $"TotalCount: {response.TotalCount}"
            );

            base.OnSuccess(request, response);
        }
    }
}
