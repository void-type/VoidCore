using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Model.Authorization;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Information to start the client application
    /// /// </summary>
    public class ApplicationInfo
    {
        /// <summary>
        /// The UI-friendly name of the application
        /// </summary>
        /// <value></value>
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

        /// <summary>
        /// The UI-friendly user name
        /// </summary>
        /// <value></value>
        public string UserName { get; }

        /// <summary>
        /// Construct a new ApplicaitonInfo object.
        /// </summary>
        /// <param name="applicationSettings">General application settings</param>
        /// <param name="httpContextAccessor">The HttpContext</param>
        /// <param name="antiforgery">The ASPNET antiforgery object</param>
        /// <param name="currentUser">UI-friendly user name</param>
        public ApplicationInfo(ApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUser currentUser)
        {
            ApplicationName = applicationSettings.Name ?? "Application";
            UserName = currentUser.Name;
            AntiforgeryToken = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).RequestToken;
            AntiforgeryTokenHeaderName = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).HeaderName;
        }
    }
}
