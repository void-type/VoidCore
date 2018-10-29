using VoidCore.Model.Domain;
using VoidCore.Model.Responses.Files;

namespace VoidCore.Model.Logging
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    public class SimpleFileEventLogger<TRequest> : FallibleEventLogger<TRequest, ISimpleFile>
    {
        /// <summary>
        /// Construct a new IItemSet logger.
        /// </summary>
        /// <param name="logger">A service to log to</param>
        /// <returns></returns>
        public SimpleFileEventLogger(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<ISimpleFile> successfulResult)
        {
            Logger.Info(
                $"FileName: {successfulResult.Value.Name}"
            );

            base.OnSuccess(request, successfulResult);
        }
    }
}
