using System.Collections.Generic;

namespace VoidCore.Model.ClientApp
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
        /// <value></value>
        Dictionary<string, List<string>> AuthorizationPolicies { get; }
    }
}
