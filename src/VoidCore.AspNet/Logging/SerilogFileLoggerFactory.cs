using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// A factory for obtaining a new Serilog logger.
    /// </summary>
    public class SerilogFileLoggerFactory
    {
        /// <summary>
        /// Create a new Serilog ILogger using an absolute path. This can be OS sensitive.
        /// </summary>
        /// <param name="settings">Settings for the logger</param>
        /// <typeparam name="TClass">A class within the assembly to be logged.</typeparam>
        /// <returns>A Serilog ILogger instance</returns>
        public static ILogger Create<TClass>(LoggingSettings settings)
        {
            var frameworkLoggingLevel = settings.SuppressFrameworkWarnings ?
                LogEventLevel.Error :
                LogEventLevel.Warning;

            var logFilePath = string.IsNullOrWhiteSpace(settings.LogFilePath) ?
                GetDefaultPath<TClass>() :
                settings.LogFilePath;

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", frameworkLoggingLevel)
                .MinimumLevel.Override("System", frameworkLoggingLevel)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval : RollingInterval.Day,
                    retainedFileCountLimit : settings.DaysToRetain,
                    fileSizeLimitBytes : 10000000,
                    rollOnFileSizeLimit : true)
                .CreateLogger();
        }

        private static string GetDefaultPath<TClass>()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var logPath = (isWindows ? Path.GetPathRoot(Environment.CurrentDirectory) : "/") + "webAppLogs";
            var assemblyName = typeof(TClass).Assembly.GetName().Name;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return $"{logPath}/{assemblyName}/{assemblyName}-{environmentName}_.log";
        }
    }
}
