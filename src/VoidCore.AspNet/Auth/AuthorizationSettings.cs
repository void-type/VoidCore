using System.Collections.Generic;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Application authorization settings that are pulled from configuration.
    /// </summary>
    public class AuthorizationSettings
    {
        /// <summary>
        /// Policies represented as a key of policyName and names of allowed roles.
        /// </summary>
        public Dictionary<string, List<string>> Policies { get; init; } = new Dictionary<string, List<string>>();
    }
}
