using VoidCore.Domain.Guards;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// General application settings that are pulled from configuration.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// The UI-friendly name of the application.
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// The base URL that the application is accessed from.
        /// </summary>
        public string BaseUrl { get; init; } = string.Empty;

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
