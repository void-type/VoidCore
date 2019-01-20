namespace VoidCore.AspNet.Logging
{
    /// <summary>
    /// Settings for the logger
    /// </summary>
    public class LoggingSettings
    {
        /// <summary>
        /// The path to the logging file.
        /// </summary>
        public string LogFilePath { get; private set; }

        /// <summary>
        /// Don't log Microsoft and System warnings.
        /// </summary>
        public bool SuppressFrameworkWarnings { get; private set; }

        /// <summary>
        /// How many days to retain the logs before deleting them.
        /// </summary>
        public int DaysToRetain { get; private set; }

        /// <summary>
        /// Create settings for the file logger.
        /// </summary>
        public LoggingSettings() { }

        /// <summary>
        /// Create settings for the file logger.
        /// </summary>
        /// <param name="logFilePath">The full path to the file. Serilog will append the date before the extension.
        /// If left null, a default path will be chosen</param>
        /// <param name="suppressFrameworkWarnings">Suppress warnings logged by Microsoft and System</param>
        /// <param name="daysToRetain">How many days to keep logged files</param>
        public LoggingSettings(string logFilePath = null, bool suppressFrameworkWarnings = false, int daysToRetain = 30)
        {
            LogFilePath = logFilePath;
            SuppressFrameworkWarnings = suppressFrameworkWarnings;
            DaysToRetain = daysToRetain;
        }
    }
}
