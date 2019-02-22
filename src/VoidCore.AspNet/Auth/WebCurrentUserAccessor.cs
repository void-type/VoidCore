using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using VoidCore.Model.Auth;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Access the current user via HttpContext. Accessors can be added to DI as a singleton.
    /// </summary>
    public class WebCurrentUserAccessor : ICurrentUserAccessor
    {
        /// <inherit/>
        public DomainUser User
        {
            get
            {
                var currentUser = _httpContextAccessor.HttpContext.User;

                var authorizedAs = _authorizationSettings.Policies
                    .Where(policy => _authorizationService.AuthorizeAsync(currentUser, policy.Key).Result.Succeeded)
                    .Select(policy => policy.Key);

                var name = _userNameFormatter.Format(currentUser.Identity.Name);

                return new DomainUser(name, authorizedAs);
            }
        }

        /// <summary>
        /// Create a new current user accessor
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="userNameFormatter">A formatter for the user names</param>
        /// <param name="authorizationService">Policy checker for users</param>
        /// <param name="authorizationSettings">The application's authorization settings</param>
        public WebCurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserNameFormatStrategy userNameFormatter, IAuthorizationService authorizationService, AuthorizationSettings authorizationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _userNameFormatter = userNameFormatter;
            _authorizationService = authorizationService;
            _authorizationSettings = authorizationSettings;
        }

        private readonly AuthorizationSettings _authorizationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserNameFormatStrategy _userNameFormatter;
    }
}
