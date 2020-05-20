using VoidCore.Model.Responses.Files;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    public class SimpleFileEventLogger<TRequest> : FallibleEventLogger<TRequest, SimpleFile>
    {
        /// <summary>
        /// Construct a new IItemSet logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        public SimpleFileEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, SimpleFile response)
        {
            Logger.Info($"FileName: {response.Name}");

            base.OnSuccess(request, response);
        }
    }
}
