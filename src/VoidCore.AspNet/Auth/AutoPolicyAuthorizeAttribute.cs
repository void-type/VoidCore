using Microsoft.AspNetCore.Authorization;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Derivatives of this class will have their policies set by convention from the name of the class.
    /// Ex: UserOnly will authorize the policy of "User"
    /// </summary>
    public class AutoPolicyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Construct a new AutoNamingAuthorizeAttribute with a policy set by convention.
        /// </summary>
        protected AutoPolicyAuthorizeAttribute()
        {
            Policy = GetType().GetTypeNameWithoutEnding("only");
        }
    }
}
