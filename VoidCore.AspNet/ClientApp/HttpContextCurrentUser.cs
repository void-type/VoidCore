using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Access the current user via HttpContext.
    /// </summary>
    public class HttpContextCurrentUser : ICurrentUser
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public IEnumerable<string> AuthorizedAs { get; }

        /// <summary>
        /// Create a new current user accessor
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="userNameFormatter">A formatter for the user names</param>
        /// <param name="authorizationService">Policy checker for users</param>
        /// <param name="applicationSettings">The application's authorization settings</param>
        public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor, IUserNameFormatStrategy userNameFormatter, IAuthorizationService authorizationService, IApplicationSettings applicationSettings)
        {
            var user = httpContextAccessor.HttpContext.User;

            Name = userNameFormatter.Format(user.Identity.Name);

            AuthorizedAs = applicationSettings.AuthorizationPolicies
                .Where(policy => authorizationService.AuthorizeAsync(user, policy.Key).Result.Succeeded)
                .Select(policy => policy.Key);
        }
    }
}
