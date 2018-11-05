using Microsoft.AspNetCore.Authorization;
using VoidCore.AspNet.Configuration;

namespace VoidCore.AspNet.Attributes
{
    /// <summary>
    /// Derivatives of this class will have their policies set by convention of the name of the class.
    /// Ex: UsersOnly will authorize the policy of "Users"
    /// </summary>
    public class AutoPolicyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Construct a new AutoNamingAuthorizeAttribute with a policy set by convention.
        /// </summary>
        public AutoPolicyAuthorizeAttribute()
        {
            Policy = ConfigHelpers.StripEndingFromType(GetType(), "only");
        }
    }
}
