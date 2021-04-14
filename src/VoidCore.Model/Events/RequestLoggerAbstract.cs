using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Events
{
    /// <inheritdoc/>
    public abstract class RequestLoggerAbstract<T> : IRequestLogger<T>
    {
        /// <summary>
        /// A logging service.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Construct a new RequestLoggerAbstract
        /// </summary>
        /// <param name="logger">A logger</param>
        protected RequestLoggerAbstract(ILogger logger)
        {
            Logger = logger;
        }

        /// <inheritdoc/>
        public abstract void Log(T request);
    }
}
