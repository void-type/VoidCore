using Microsoft.AspNetCore.Http;
#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
#endif
using VoidCore.Model.Configuration;

namespace VoidCore.AspNet.Configuration
{
    /// <inheritdoc/>
    public class WebAppVariables : IWebAppVariables
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationSettings _appSettings;
#if NETCOREAPP3_0
        private readonly IHostEnvironment _host;
#else
        private readonly IHostingEnvironment _host;
#endif

        /// <summary>
        /// Construct a new WebAppVariables object
        /// </summary>
        /// <param name="contextAccessor">The HttpContextAccessor</param>
        /// <param name="host">The host environment</param>
        /// <param name="appSettings">The application settings</param>
#if NETCOREAPP3_0
        public WebAppVariables(IHttpContextAccessor contextAccessor, IHostEnvironment host, ApplicationSettings appSettings)
        {
            _contextAccessor = contextAccessor;
            _appSettings = appSettings;
            _host = host;
        }
#else
        public WebAppVariables(IHttpContextAccessor contextAccessor, IHostingEnvironment host, ApplicationSettings appSettings)
        {
            _contextAccessor = contextAccessor;
            _appSettings = appSettings;
            _host = host;
        }
#endif

        /// <inheritdoc/>
        public string AppName => _appSettings.Name;

        /// <inheritdoc/>
        public string BaseUrl
        {
            get
            {
                var request = _contextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host}{request.PathBase}";
            }
        }

        /// <inheritdoc/>
        public string Environment => _host.EnvironmentName;
    }
}
