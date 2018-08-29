using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Steps;
using VoidCore.Model.Authorization;

namespace VoidCore.AspNet.Action
{
    /// <summary>
    /// Send initial information like antiforgery tokens and UI strings to start the ClientApp.
    /// </summary>
    public class RespondWithApplicationInfo : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="applicationSettings">General application settings</param>
        /// <param name="httpContextAccessor">The HttpContext</param>
        /// <param name="antiforgery">The ASPNET antiforgery object</param>
        /// <param name="currentUser">UI-friendly user name</param>
        public RespondWithApplicationInfo(ApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUser currentUser)
        {
            _applicationSettings = applicationSettings;
            _httpContext = httpContextAccessor.HttpContext;
            _antiforgery = antiforgery;
            _currentUser = currentUser;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var info = new ApplicationInfo
            {
                ApplicationName = _applicationSettings.Name ?? "Application",
                UserName = _currentUser.Name,
                AntiforgeryToken = _antiforgery.GetAndStoreTokens(_httpContext).RequestToken,
                AntiforgeryTokenHeaderName = _antiforgery.GetAndStoreTokens(_httpContext).HeaderName
            };

            respond.WithSuccess(info);
        }

        private readonly IAntiforgery _antiforgery;
        private readonly ApplicationSettings _applicationSettings;
        private readonly ICurrentUser _currentUser;
        private readonly HttpContext _httpContext;
    }
}
