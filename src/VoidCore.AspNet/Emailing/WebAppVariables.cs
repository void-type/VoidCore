using Microsoft.AspNetCore.Http;
#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#else
using Microsoft.AspNetCore.Hosting;
#endif
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Emailing;

namespace VoidCore.AspNet.Emailing
{
    public class WebAppVariables : IAppVariables
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationSettings _appSettings;
#if NETCOREAPP3_0
        private readonly IHostEnvironment _host;
#else
        private readonly IHostingEnvironment _host;
#endif

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
        public string AppName => _appSettings.Name;

        public string BaseUrl
        {
            get
            {
                var request = _contextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host}{request.PathBase}";
            }
        }

        public string Environment => _host.EnvironmentName;
    }
}
