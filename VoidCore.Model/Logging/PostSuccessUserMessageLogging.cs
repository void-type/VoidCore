using VoidCore.Model.DomainEvents;
using VoidCore.Model.Responses.Message;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set page.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TId">The type of items in the item set</typeparam>
    public class PostSuccessUserMessageLogging<TRequest, TId> : FallibleLogging<TRequest, PostSuccessUserMessage<TId>>
    {
        /// <summary>
        /// Construct a new IItemSetPage logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public PostSuccessUserMessageLogging(ILoggingService logger) : base(logger) { }

        /// <summary>
        /// Log meta information about the item set page.
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="successfulResult">The successful result</param>
        public override void OnSuccess(TRequest request, IResult<PostSuccessUserMessage<TId>> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
