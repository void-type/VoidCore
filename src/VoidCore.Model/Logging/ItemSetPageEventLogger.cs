using VoidCore.Model.Domain;
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
        public override void OnSuccess(TRequest request, IResult<IItemSetPage<TEntity>> successfulResult)
        {
            Logger.Info(
                $"Count: {successfulResult.Value.Count}",
                $"Page: {successfulResult.Value.Page}",
                $"Take: {successfulResult.Value.Take}",
                $"TotalCount: {successfulResult.Value.TotalCount}"
            );

            base.OnSuccess(request, successfulResult);
        }
    }
}
