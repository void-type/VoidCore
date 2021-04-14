using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// Log meta information about the SimpleFile.
    /// </summary>
    /// <typeparam name="TRequest">The request type of the event.</typeparam>
    public class SimpleFileEventLogger<TRequest> : FallibleEventLoggerAbstract<TRequest, SimpleFile>
    {
        /// <inheritdoc/>
        public SimpleFileEventLogger(ILogger<SimpleFileEventLogger<TRequest>> logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(TRequest request, SimpleFile response)
        {
            Logger.LogInformation("Responded with SimpleFile. FileName: {FileName} FileSizeBytes: {FileSizeBytes} bytes",
                response.Name,
                response.Content.AsBytes.Length);

            base.OnSuccess(request, response);
        }
    }
}
