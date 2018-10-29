using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Access the current user via HttpContext.
    /// </summary>
    public class WebCurrentUserAccessor : ICurrentUserAccessor
    {
        /// <inheritdoc/>
        public IEnumerable<string> AuthorizedAs => _applicationSettings.AuthorizationPolicies
            .Where(policy => _authorizationService.AuthorizeAsync(User, policy.Key).Result.Succeeded)
            .Select(policy => policy.Key);

        /// <inheritdoc/>
        public string Name => _userNameFormatter.Format(User.Identity.Name);

        /// <summary>
        /// Create a new current user accessor
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="userNameFormatter">A formatter for the user names</param>
        /// <param name="authorizationService">Policy checker for users</param>
        /// <param name="applicationSettings">The application's authorization settings</param>
        public WebCurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserNameFormatStrategy userNameFormatter, IAuthorizationService authorizationService, IApplicationSettings applicationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _userNameFormatter = userNameFormatter;
            _authorizationService = authorizationService;
            _applicationSettings = applicationSettings;
        }

        private readonly IApplicationSettings _applicationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserNameFormatStrategy _userNameFormatter;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
    }
}