using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Information to start the client application
    /// </summary>
    public interface IApplicationInfo
    {

        /// <summary>
        /// The UI-friendly name of the application
        /// </summary>
        /// <value></value>
        string ApplicationName { get; }

        /// <summary>
        /// The value of the header antiforgery token
        /// </summary>
        /// <value></value>
        string AntiforgeryToken { get; }

        /// <summary>
        /// The header name of the antiforgery token
        /// </summary>
        /// <value></value>
        string AntiforgeryTokenHeaderName { get; }

        /// <summary>
        /// The UI-friendly user name
        /// </summary>
        /// <value></value>
        string UserName { get; }
    }

    /// <inheritdoc/>
    public class ApplicationInfo : IApplicationInfo
    {
        /// <inheritdoc/>
        public string ApplicationName { get; }

        /// <inheritdoc/>
        public string AntiforgeryToken { get; }

        /// <inheritdoc/>
        public string AntiforgeryTokenHeaderName { get; }

        /// <inheritdoc/>
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
