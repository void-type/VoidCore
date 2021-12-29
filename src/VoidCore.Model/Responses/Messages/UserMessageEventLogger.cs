using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses.Messages;

/// <summary>
/// Log meta information about the UserMessage.
/// </summary>
/// <typeparam name="TRequest">The request type of the event.</typeparam>
public class UserMessageEventLogger<TRequest> : FallibleEventLoggerAbstract<TRequest, UserMessage>
{
    /// <inheritdoc cref="FallibleEventLoggerAbstract{TRequest, TResponse}"/>
    public UserMessageEventLogger(ILogger<UserMessageEventLogger<TRequest>> logger) : base(logger) { }

    /// <inheritdoc/>
    protected override void OnSuccess(TRequest request, UserMessage response)
    {
        Logger.LogInformation("Responded with UserMessage. Message: {Message}",
            response.Message);

        base.OnSuccess(request, response);
    }
}
