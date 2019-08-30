using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// Defaults for setting up logging.
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Return a default file path.
        /// An example path is C:\webAppLogs\MyProgram\MyProgram-Development_.log
        /// if the current directory is on the C:\ drive and the parameter
        /// assemblyName of "MyProgram" running in the ASPNETCORE environment of Development.
        /// </summary>
        /// <param name="assemblyName">The name of the logged program.</param>
        /// <returns>A default file path</returns>
        public static string FilePath(string assemblyName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var logPath = (isWindows ? Path.GetPathRoot(Environment.CurrentDirectory) : "/") + "webAppLogs";
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return $"{logPath}/{assemblyName}/{assemblyName}-{environmentName}_.log";
        }

        /// <summary>
        /// Return a default file path.
        /// An example path is C:\webAppLogs\MyProgram\MyProgram-Development_.log
        /// if the current directory is on the C:\ drive and the type parameter is a
        /// type within the assembly of "MyProgram" running in the ASPNETCORE environment of Development.
        /// </summary>
        /// <typeparam name="T">A type within the assembly being logged.</typeparam>
        /// <returns>A default file path</returns>
        public static string FilePath<T>()
        {
            var assemblyName = typeof(T).Assembly.GetName().Name;
            return FilePath(assemblyName);
        }
    }
}
