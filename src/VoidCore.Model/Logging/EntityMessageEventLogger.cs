using VoidCore.Model.Responses.Messages;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the EntityMessage.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TId">The type of the entity id</typeparam>
    public class EntityMessageEventLogger<TRequest, TId> : FallibleEventLogger<TRequest, EntityMessage<TId>>
    {
        /// <summary>
        /// Construct a new logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public EntityMessageEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, EntityMessage<TId> response)
        {
            Logger.Info(
                $"Message: {response.Message}",
                $"EntityId: {response.Id}"
            );

            base.OnSuccess(request, response);
        }
    }
}
