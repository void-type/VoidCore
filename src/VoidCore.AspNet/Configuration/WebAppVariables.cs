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
                _contextAccessor.HttpContext.EnsureNotNull(nameof(_contextAccessor.HttpContext));
                var request = _contextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host}{request.PathBase}";
            }
        }

        /// <inheritdoc/>
        public string Environment => _host.EnvironmentName;
    }
}
