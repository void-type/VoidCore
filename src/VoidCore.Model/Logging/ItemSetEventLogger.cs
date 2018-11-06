using VoidCore.Model.Domain;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.Model.Logging
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
        public override void OnSuccess(TRequest request, IResult<IItemSet<TEntity>> successfulResult)
        {
            Logger.Info(
                $"Count: {successfulResult.Value.Count}"
            );

            base.OnSuccess(request, successfulResult);
        }
    }
}
