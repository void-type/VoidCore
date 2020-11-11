using VoidCore.Domain.Guards;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// General application settings that are pulled from configuration.
    /// </summary>
    public class ApplicationSettings
    {
        private string? _name;
        private string? _baseUrl;

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
        public string Name
        {
            get { return _name!; }
            private set { _name = value.EnsureNotNull(nameof(Name)); }
        }

        /// <summary>
        /// The base URL that the application is accessed from.
        /// </summary>
        public string BaseUrl
        {
            get { return _baseUrl!; }
            private set { _baseUrl = value.EnsureNotNull(nameof(BaseUrl)); }
        }

        /// <summary>
        /// Check the state of this configuration and throw an exception if it is invalid.
        /// </summary>
        public void Validate()
        {
            Name.EnsureNotNullOrEmpty(nameof(Name), "Property not found in application configuration.");
            BaseUrl.EnsureNotNullOrEmpty(nameof(BaseUrl), "Property not found in application configuration.");
        }
    }
}
