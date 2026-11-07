using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses.Files;

/// <summary>
/// Log meta information about the SimpleFile.
/// </summary>
/// <typeparam name="TRequest">The request type of the event.</typeparam>
public class SimpleFileEventLogger<TRequest> : FallibleEventLoggerAbstract<TRequest, SimpleFile>
{
    /// <inheritdoc cref="FallibleEventLoggerAbstract{TRequest, TResponse}"/>
    public SimpleFileEventLogger(ILogger<SimpleFileEventLogger<TRequest>> logger) : base(logger) { }

    /// <inheritdoc/>
    protected override void OnSuccess(TRequest request, SimpleFile response)
    {
        ResponseLogMessages.RespondedWithSimpleFile(Logger, response.Name, response.Content.AsBytes.Length);

        base.OnSuccess(request, response);
    }
}
