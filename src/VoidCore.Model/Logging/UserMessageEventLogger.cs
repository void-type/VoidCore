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
        public UserMessageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, UserMessage response)
        {
            Logger.Info($"Message: {response.Message}");

            base.OnSuccess(request, response);
        }
    }
}
