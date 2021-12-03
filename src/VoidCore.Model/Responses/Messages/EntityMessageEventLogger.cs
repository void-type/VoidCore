using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses.Messages;

/// <summary>
/// Log meta information about the EntityMessage.
/// </summary>
/// <typeparam name="TRequest">The request type of the event.</typeparam>
/// <typeparam name="TId">The type of the entity id</typeparam>
public class EntityMessageEventLogger<TRequest, TId> : FallibleEventLoggerAbstract<TRequest, EntityMessage<TId>>
{
    /// <inheritdoc/>
    public EntityMessageEventLogger(ILogger<EntityMessageEventLogger<TRequest, TId>> logger) : base(logger) { }

    /// <inheritdoc/>
    protected override void OnSuccess(TRequest request, EntityMessage<TId> response)
    {
        Logger.LogInformation("Responded with EntityMessage. Message: {Message} EntityId: {EntityId}",
            response.Message,
            response.Id);

        base.OnSuccess(request, response);
    }
}
