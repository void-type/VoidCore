using Microsoft.AspNetCore.Authorization;
using VoidCore.AspNet.Configuration;

namespace VoidCore.AspNet.Authorization
{
    /// <summary>
    /// Derivatives of this class will have their policies set by convention of the name of the class.
    /// Ex: UsersOnly will authorize the policy of "Users"
    /// </summary>
    public class AutoNamingAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Construct a new AutoNamingAuthorizeAttribute with a policy set by convention.
        /// </summary>
        public AutoNamingAuthorizeAttribute() : base()
        {
            this.Policy = ConfigHelpers.StripEndingFromType(this.GetType(), "only");
        }
    }
}
