using Microsoft.Extensions.Hosting;
using VoidCore.Model.Configuration;

namespace VoidCore.AspNet.Configuration
{
    /// <inheritdoc/>
    public class WebAppVariables : IWebAppVariables
    {
        private readonly ApplicationSettings _appSettings;
        private readonly IHostEnvironment _host;

        /// <summary>
        /// Construct a new WebAppVariables object.
        /// </summary>
        /// <param name="host">The host environment</param>
        /// <param name="appSettings">The application settings</param>
        public WebAppVariables(IHostEnvironment host, ApplicationSettings appSettings)
        {
            _appSettings = appSettings;
            _host = host;
        }

        /// <inheritdoc/>
        public string AppName => _appSettings.Name;

        /// <inheritdoc/>
        public string BaseUrl => _appSettings.BaseUrl;

        /// <inheritdoc/>
        public string Environment => _host.EnvironmentName;
    }
}
