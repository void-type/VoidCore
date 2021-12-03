using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Responses;

/// <summary>
/// A base post processor that logs event failures.
/// </summary>
/// <typeparam name="TRequest">The request type of the event.</typeparam>
/// <typeparam name="TResponse">The response type of the event.</typeparam>
public abstract class FallibleEventLoggerAbstract<TRequest, TResponse> : PostProcessorAbstract<TRequest, TResponse>
{
    /// <summary>
    /// Instance of a logging service.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Construct a new post processor
    /// </summary>
    /// <param name="logger">The ILogger to log to</param>
    protected FallibleEventLoggerAbstract(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Log failures of the IFallible. If other you are overriding this method, be sure to call base() to invoke this
    /// default behavior.
    /// </summary>
    /// <param name="request">The domain event request</param>
    /// <param name="failures">The failures of the event</param>
    protected override void OnFailure(TRequest request, IEnumerable<IFailure> failures)
    {
        var failuresList = failures.ToList();

        Logger.LogWarning("Count: {Count} Failures: {FailureMessages}",
            failuresList.Count,
            string.Join(" ", failuresList.Select(failure => failure.Message))
        );

        base.OnFailure(request, failures);
    }
}
