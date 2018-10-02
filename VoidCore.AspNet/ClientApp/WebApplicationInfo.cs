using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Information for bootstrapping a web client.
    /// </summary>
    public class WebApplicationInfo : GetApplicationInfo.IApplicationInfo
    {
        /// <inheritdoc/>
        public string ApplicationName { get; }

        /// <summary>
        /// The value of the header antiforgery token
        /// </summary>
        /// <value></value>
        public string AntiforgeryToken { get; }

        /// <summary>
        /// The header name of the antiforgery token
        /// </summary>
        /// <value></value>
        public string AntiforgeryTokenHeaderName { get; }

        /// <inheritdoc/>
        public ICurrentUser User { get; }

        /// <summary>
        /// Construct a new ApplicationInfo object.
        /// </summary>
        /// <param name="applicationSettings">Application settings pulled from configuration</param>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="antiforgery">The ASP.NET antiforgery object</param>
        /// <param name="currentUser">UI-friendly user name</param>
        public WebApplicationInfo(IApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUser currentUser)
        {
            ApplicationName = applicationSettings.Name;
            AntiforgeryToken = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).RequestToken;
            AntiforgeryTokenHeaderName = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).HeaderName;
            User = currentUser;
        }
    }
}
