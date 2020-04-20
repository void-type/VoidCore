using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using VoidCore.Model.Configuration;
using VoidCore.Domain.Guards;

namespace VoidCore.AspNet.Configuration
{
    /// <inheritdoc/>
    public class WebAppVariables : IWebAppVariables
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationSettings _appSettings;
        private readonly IHostEnvironment _host;

        /// <summary>
        /// Construct a new WebAppVariables object.
        /// </summary>
        /// <param name="contextAccessor">The HttpContextAccessor</param>
        /// <param name="host">The host environment</param>
        /// <param name="appSettings">The application settings</param>
        public WebAppVariables(IHttpContextAccessor contextAccessor, IHostEnvironment host, ApplicationSettings appSettings)
        {
            _contextAccessor = contextAccessor;
            _appSettings = appSettings;
            _host = host;
        }

        /// <inheritdoc/>
        public string AppName => _appSettings.Name;

        /// <inheritdoc/>
        public string BaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appSettings.BaseUrl))
                {
                    // For apps that don't have the BaseUrl in settings, we can calculate it from the request.
                    // This method does not work for hosted services that don't have a web request.
                    _contextAccessor.HttpContext.EnsureNotNull(nameof(_contextAccessor.HttpContext));
                    var request = _contextAccessor.HttpContext.Request;
                    return $"{request.Scheme}://{request.Host}{request.PathBase}";
                }

                return _appSettings.BaseUrl;
            }
        }

        /// <inheritdoc/>
        public string Environment => _host.EnvironmentName;
    }
}
