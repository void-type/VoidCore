using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using VoidCore.Model.Auth;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Access the current user via HttpContext. Accessors can be added to DI as a singleton.
    /// </summary>
    public class WebCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly AuthorizationSettings _authorizationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserNameFormatStrategy _userNameFormatter;

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

        /// <inheritdoc/>
        public DomainUser User
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return new DomainUser(_userNameFormatter.Format(null), Array.Empty<string>());
                }

                var currentUser = _httpContextAccessor.HttpContext.User;

                var authorizedAs = _authorizationSettings.Policies
                    .Where(policy => _authorizationService.AuthorizeAsync(currentUser, null, policy.Key).GetAwaiter().GetResult().Succeeded)
                    .Select(policy => policy.Key);

                var name = _userNameFormatter.Format(currentUser.Identity.Name);

                return new DomainUser(name, authorizedAs);
            }
        }
    }
}
