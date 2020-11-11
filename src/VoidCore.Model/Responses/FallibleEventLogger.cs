using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses
{
    /// <summary>
    /// A base post processor that logs IFallible failures to a string logger.
    /// </summary>
    /// <typeparam name="TRequest">The request type of the event.</typeparam>
    /// <typeparam name="TResponse">The response type of the event.</typeparam>
    public class FallibleEventLogger<TRequest, TResponse> : PostProcessorAbstract<TRequest, TResponse>
    {
        /// <summary>
        /// Instance of a logging service.
        /// </summary>
        protected readonly ILoggingService Logger;

        /// <summary>
        /// Construct a new post processor
        /// </summary>
        /// <param name="logger">The ILoggingService to log to</param>
        public FallibleEventLogger(ILoggingService logger)
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

            Logger.Warn(
                $"Count: {failuresList.Count}",
                $"Failures: {string.Join(" ", failuresList.Select(failure => failure.Message))}");

            base.OnFailure(request, failuresList);
        }
    }
}
