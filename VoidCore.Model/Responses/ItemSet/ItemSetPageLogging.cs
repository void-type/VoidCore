using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses.ItemSet
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set page.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TEntity">The type of items in the item set</typeparam>
    public class ItemSetPageLogging<TRequest, TEntity> : FallibleLogging<TRequest, IItemSetPage<TEntity>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public ItemSetPageLogging(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<IItemSetPage<TEntity>> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
