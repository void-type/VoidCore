using VoidCore.Model.DomainEvents;
using VoidCore.Model.Responses.ItemSet;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The type of items in the item set</typeparam>
    public class ItemSetLogging<TRequest, TResponse> : FallibleLogging<TRequest, IItemSet<TResponse>>
    {
        /// <summary>
        /// Construct a new IItemSet logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public ItemSetLogging(ILoggingService logger) : base(logger) { }

        /// <summary>
        /// Log meta information about the item set.
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="successfulResult">The successful result</param>
        public override void OnSuccess(TRequest request, IFallible<IItemSet<TResponse>> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
