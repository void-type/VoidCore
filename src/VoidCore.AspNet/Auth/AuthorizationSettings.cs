using System.Collections.Generic;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Application authorization settings that are pulled from configuration.
    /// </summary>
    public class AuthorizationSettings
    {
        /// <summary>
        /// Constructor used by AspNet to instantiate using private setters.
        /// </summary>
        public AuthorizationSettings() { }

        /// <summary>
        /// Construct a new settings.
        /// </summary>
        /// <param name="policies">The authorization policies of the application.</param>
        public AuthorizationSettings(Dictionary<string, List<string>> policies)
        {
            Policies = policies;
        }

        /// <summary>
        /// Policies represented as a key of policyName and names of allowed roles.
        /// </summary>
        public Dictionary<string, List<string>> Policies { get; private set; } = new Dictionary<string, List<string>>();
    }
}
