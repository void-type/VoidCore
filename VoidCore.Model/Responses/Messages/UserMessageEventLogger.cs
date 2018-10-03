using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses.Messages
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the usermessage.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    public class UserMessageEventLogger<TRequest> : FallibleEventLogger<TRequest, UserMessage>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public UserMessageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<UserMessage> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
