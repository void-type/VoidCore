using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.ClientApp
{
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
        public ICurrentUser User { get; }

        /// <summary>
        /// Construct a new ApplicaitonInfo object.
        /// </summary>
        /// <param name="applicationSettings">General application settings</param>
        /// <param name="httpContextAccessor">The HttpContext</param>
        /// <param name="antiforgery">The ASPNET antiforgery object</param>
        /// <param name="currentUser">UI-friendly user name</param>
        public ApplicationInfo(IApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUser currentUser)
        {
            ApplicationName = applicationSettings.Name;
            AntiforgeryToken = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).RequestToken;
            AntiforgeryTokenHeaderName = antiforgery.GetAndStoreTokens(httpContextAccessor.HttpContext).HeaderName;
            User = currentUser;
        }
    }
}
