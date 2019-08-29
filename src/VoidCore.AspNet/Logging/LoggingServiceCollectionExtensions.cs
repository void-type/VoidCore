using Microsoft.Extensions.DependencyInjection;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// Configuration for logging.
    /// </summary>
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// Add logging domain to framework logging adapters.
        /// </summary>
        /// <param name="services">This service collection</param>
        public static void AddWebLoggingAdapters(this IServiceCollection services)
        {
            services.AddSingleton<ILoggingStrategy, HttpRequestLoggingStrategy>();
            services.AddSingleton<ILoggingService, MicrosoftLoggerAdapter>();
        }
    }
}
