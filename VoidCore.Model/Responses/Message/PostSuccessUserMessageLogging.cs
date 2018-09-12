using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses.Message
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the postsuccessusermessage.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TId">The type of the entity id</typeparam>
    public class PostSuccessUserMessageLogging<TRequest, TId> : FallibleLogging<TRequest, PostSuccessUserMessage<TId>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public PostSuccessUserMessageLogging(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<PostSuccessUserMessage<TId>> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
