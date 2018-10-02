using System.Linq;
using VoidCore.Model.Logging;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// A base post processor that logs IFallible failures to a string logger.
    /// </summary>
    /// <typeparam name="TRequest">The request type of the event.</typeparam>
    /// <typeparam name="TResponse">The response type of the event.</typeparam>
    public class FallibleLogging<TRequest, TResponse> : PostProcessorAbstract<TRequest, TResponse>
    {
        /// <summary>
        /// Construct a new post processor
        /// </summary>
        /// <param name="logger">The ILoggingService to log to</param>
        public FallibleLogging(ILoggingService logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Log failures of the IFallible. If other you are overriding this method, be sure to call base() to
        /// invoke this default behavior.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        public override void OnFailure(TRequest request, IResult result)
        {
            Logger.Warn(
                $"Count: {result.Failures.Count()}",
                $"Failures: {string.Join(" ", result.Failures.Select(failure => failure.Message))}");
            base.OnFailure(request, result);
        }

        /// <summary>
        /// Instance of a logging service.
        /// </summary>
        protected readonly ILoggingService Logger;
    }
}
