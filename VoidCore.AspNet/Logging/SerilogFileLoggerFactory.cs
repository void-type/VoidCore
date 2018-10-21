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
        /// <param name="logFilePath">The full path to the file. Serilog will append the date before the extension</param>
        /// <param name="suppressFrameworkWarnings">Suppress warnings logged by Microsoft and System</param>
        /// <param name="daysToRetain">How many days to keep logged files</param>
        /// <returns>A Serilog ILogger instance</returns>
        public static ILogger Create(string logFilePath = null, bool suppressFrameworkWarnings = false, int daysToRetain = 30)
        {
            var frameworkLoggingLevel = suppressFrameworkWarnings ? LogEventLevel.Error : LogEventLevel.Warning;

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", frameworkLoggingLevel)
                .MinimumLevel.Override("System", frameworkLoggingLevel)
                .Enrich.FromLogContext()
                .WriteTo.File(logFilePath,
                    rollingInterval : RollingInterval.Day,
                    retainedFileCountLimit : daysToRetain,
                    fileSizeLimitBytes : 10000000,
                    rollOnFileSizeLimit : true)
                .CreateLogger();
        }

        /// <summary>
        /// Creates an ILogger using a default path.
        /// The default path is at the root of the current directory in webAppLogs.
        /// IE: /webAppLogs/ on *nix and C:\webAppLogs or D:\webAppLogs (if IIS is moved) on Windows.
        /// </summary>
        /// <param name="suppressFrameworkWarnings">Suppress warnings logged by Microsoft and System</param>
        /// <param name="daysToRetain">How many days to keep logged files</param>
        /// <typeparam name="TClass">The type of a class in the main assembly. Used to determine root directory.</typeparam>
        /// <returns>A Serilog ILogger instance</returns>
        public static ILogger Create<TClass>(bool suppressFrameworkWarnings = false, int daysToRetain = 30) where TClass : class
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var logPath = (isWindows ? Path.GetPathRoot(Environment.CurrentDirectory) : "/") + "webAppLogs";
            var assemblyName = typeof(TClass).Assembly.GetName().Name;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logFilePath = $"{logPath}/{assemblyName}/{assemblyName}-{environmentName}_.log";

            return Create(logFilePath, suppressFrameworkWarnings, daysToRetain);
        }
    }
}
