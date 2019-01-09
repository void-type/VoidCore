using System.Collections.Generic;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// General application settings that are pulled from configuration.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Policies represented as a key of policyName and names of allowed roles.
        /// </summary>
        public Dictionary<string, List<string>> AuthorizationPolicies { get; private set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Constructor used by AspNet to instantiate using private setters.
        /// </summary>
        public ApplicationSettings() { }

        /// <summary>
        /// Construct a new settings.
        /// </summary>
        /// <param name="name">The UI-friendly name of the application.</param>
        /// <param name="authorizationPolicies">The authorization policies of the application.</param>
        public ApplicationSettings(string name, Dictionary<string, List<string>> authorizationPolicies)
        {
            Name = name;
            AuthorizationPolicies = authorizationPolicies;
        }
    }
}
