using VoidCore.Model.Configuration;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Configuration;

/// <summary>
/// General application settings that are pulled from configuration.
/// </summary>
public class WebApplicationSettings : ApplicationSettings
{
    /// <summary>
    /// The base URL that the application is accessed from.
    /// </summary>
    public string BaseUrl { get; init; } = string.Empty;

    /// <summary>
    /// Check the state of this configuration and throw an exception if it is invalid.
    /// </summary>
    public override void Validate()
    {
        base.Validate();
        BaseUrl.EnsureNotNullOrEmpty(nameof(BaseUrl), "Property not found in application configuration.");
    }
}
