using System.Collections.Generic;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Application settings bound from configuation for creating new authorization policies.
    /// </summary>
    public class AuthorizationSettings
    {
        /// <summary>
        /// The configuration section name to pull these settings from.
        /// </summary>
        public static string SectionName => "Authorization";

        /// <summary>
        /// Policies represented as a key of policyName and names of allowed roles.
        /// </summary>
        /// <value></value>
        public Dictionary<string, List<string>> Roles { get; set; }
    }
}
