using System.Collections.Generic;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// General application settings that are pulled from configuration.
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Policies represented as a key of policyName and names of allowed roles.
        /// </summary>
        Dictionary<string, List<string>> AuthorizationPolicies { get; }
    }
}