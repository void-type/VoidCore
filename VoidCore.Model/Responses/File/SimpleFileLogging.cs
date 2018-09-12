using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace VoidCore.Model.Responses.File
{
    /// <inheritdoc/>
    /// <summary>
    /// Log meta information about the item set.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    public class SimpleFileLogging<TRequest> : FallibleLogging<TRequest, ISimpleFile>
    {
        /// <summary>
        /// Construct a new IItemSet logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public SimpleFileLogging(ILoggingService logger) : base(logger) { }

        /// <inheritdoc/>
        public override void OnSuccess(TRequest request, IResult<ISimpleFile> successfulResult)
        {
            Logger.Info(successfulResult.Value.GetLogText());
            base.OnSuccess(request, successfulResult);
        }
    }
}
