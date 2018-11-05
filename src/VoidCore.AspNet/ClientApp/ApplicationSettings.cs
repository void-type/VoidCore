using System.Collections.Generic;

namespace VoidCore.AspNet.ClientApp
{
    /// <inheritdoc/>
    public class ApplicationSettings : IApplicationSettings
    {
        /// <inheritdoc/>
        public string Name { get; private set; }

        /// <inheritdoc/>
        public Dictionary<string, List<string>> AuthorizationPolicies { get; private set; }
    }
}
