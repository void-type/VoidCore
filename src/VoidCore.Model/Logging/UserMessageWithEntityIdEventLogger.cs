using VoidCore.Model.Responses.Messages;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the UserMessageWithEntityId.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TId">The type of the entity id</typeparam>
    public class UserMessageWithEntityIdEventLogger<TRequest, TId> : FallibleEventLogger<TRequest, UserMessageWithEntityId<TId>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public UserMessageWithEntityIdEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, UserMessageWithEntityId<TId> response)
        {
            Logger.Info(
                $"Message: {response.Message}",
                $"EntityId: {response.Id.ToString()}"
            );

            base.OnSuccess(request, response);
        }
    }
}
