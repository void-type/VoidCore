using Microsoft.AspNetCore.Http;
using VoidCore.Model.Authorization;

namespace VoidCore.AspNet.Authorization
{
    /// <summary>
    /// Access the current user via HttpContext.
    /// </summary>
    public class HttpContextCurrentUser : ICurrentUser
    {
        /// <inheritdoc/>
        public string Name => _userNameFormatter.Format(_httpContext.User.Identity.Name);

        /// <summary>
        /// Create a new current user accessor
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
        /// <param name="userNameFormatter">A formatter for the user names</param>
        public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor, IUserNameFormatter userNameFormatter)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userNameFormatter = userNameFormatter;
        }

        private readonly HttpContext _httpContext;
        private readonly IUserNameFormatter _userNameFormatter;
    }
}
