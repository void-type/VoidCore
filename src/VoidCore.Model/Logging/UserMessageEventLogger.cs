using VoidCore.Model.Domain;
using VoidCore.Model.Responses.Messages;

namespace VoidCore.Model.Logging
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
        /// <param name="logger">A service to log to</param>
        /// <returns></returns>
        public UserMessageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<UserMessage> successfulResult)
        {
            Logger.Info(
                $"Message: {successfulResult.Value.Message}"
            );

            base.OnSuccess(request, successfulResult);
        }
    }
}
