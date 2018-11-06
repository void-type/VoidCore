using VoidCore.Model.Domain;
using VoidCore.Model.Responses.Messages;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the postsuccessusermessage.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TId">The type of the entity id</typeparam>
    public class PostSuccessUserMessageEventLogger<TRequest, TId> : FallibleEventLogger<TRequest, PostSuccessUserMessage<TId>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public PostSuccessUserMessageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<PostSuccessUserMessage<TId>> successfulResult)
        {
            Logger.Info(
                $"Message: {successfulResult.Value.Message}",
                $"EntityId: {successfulResult.Value.Id.ToString()}"
            );

            base.OnSuccess(request, successfulResult);
        }
    }
}
