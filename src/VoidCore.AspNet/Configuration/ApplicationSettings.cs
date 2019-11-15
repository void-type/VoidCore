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
        public ApplicationSettings(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name { get; private set; }
    }
}
