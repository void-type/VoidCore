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
    public class HttpContextCurrentUser : ICurrentUser
    {

        /// <inheritdoc/>
        public string Name => _userNameFormatter.Format(_user.Identity.Name);

        /// <inheritdoc/>
        public IEnumerable<string> AuthorizedAs => _applicationSettings.AuthorizationPolicies
            .Where(policy => _authorizationService.AuthorizeAsync(_user, policy.Key).Result.Succeeded)
            .Select(policy => policy.Key);

        /// <summary>
        /// Create a new current user accessor
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="userNameFormatter">A formatter for the user names</param>
        /// <param name="authorizationService">Policy checker for users</param>
        /// <param name="applicationSettings">The application's authorization settings</param>
        public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor, IUserNameFormatStrategy userNameFormatter, IAuthorizationService authorizationService, IApplicationSettings applicationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _userNameFormatter = userNameFormatter;
            _authorizationService = authorizationService;
            _applicationSettings = applicationSettings;
        }

        private ClaimsPrincipal _user => _httpContextAccessor.HttpContext.User;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserNameFormatStrategy _userNameFormatter;
        private readonly IAuthorizationService _authorizationService;
        private readonly IApplicationSettings _applicationSettings;
    }
}
