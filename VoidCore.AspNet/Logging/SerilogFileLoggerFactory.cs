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
        /// This logger will log to the supplied path with rolling file logs that delete after 15 days.
        /// </summary>
        /// <param name="logFilePath">The full path to the file. Serilog will append the date before the extension</param>
        /// <param name="suppressFrameworkWarnings">Suppress warnings logged by Microsoft and System</param>
        /// <returns>A Serilog ILogger instance</returns>
        public static ILogger Create(string logFilePath, bool suppressFrameworkWarnings = false)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", suppressFrameworkWarnings ? LogEventLevel.Error : LogEventLevel.Warning)
                .MinimumLevel.Override("System", suppressFrameworkWarnings ? LogEventLevel.Error : LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(logFilePath,
                    rollingInterval : RollingInterval.Day,
                    retainedFileCountLimit : 30,
                    fileSizeLimitBytes : 10000000,
                    rollOnFileSizeLimit : true)
                .CreateLogger();
        }

        /// <summary>
        /// Calls Create(string logFilePath) using the name of the supplied Program's assembly in the default path.
        /// The default path is at the root of the current directory in webAppLogs.
        /// IE: /webAppLogs/ on *nix and C:\webAppLogs or D:\webAppLogs (if IIS is moved) on Windows.
        /// </summary>
        /// <typeparam name="TClass">The type of a class in the main assembly. Used to determine root directory.</typeparam>
        /// <returns>A Serilog ILogger instance</returns>
        public static ILogger Create<TClass>(bool suppressExternalWarnings = false) where TClass : class
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var logPath = (isWindows ? Path.GetPathRoot(Environment.CurrentDirectory) : "/") + "webAppLogs";
            var assemblyName = typeof(TClass).Assembly.GetName().Name;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logFilePath = $"{logPath}/{assemblyName}-{environmentName}_.log";

            return Create(logFilePath, suppressExternalWarnings);
        }
    }
}
