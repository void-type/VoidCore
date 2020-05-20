namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// General application settings that are pulled from configuration.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Constructor used by AspNet to instantiate using private setters.
        /// </summary>
        public ApplicationSettings() { }

        /// <summary>
        /// Construct a new settings.
        /// </summary>
        /// <param name="name">The UI-friendly name of the application.</param>
        /// <param name="baseUrl">The base URL that the application is accessed from.</param>
        public ApplicationSettings(string name, string baseUrl)
        {
            Name = name;
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// The UI-friendly name of the application.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The base URL that the application is accessed from.
        /// </summary>
        public string BaseUrl { get; private set; }
    }
}
