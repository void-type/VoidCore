using System.Collections.Generic;

namespace VoidCore.Model.Auth
{
    /// <summary>
    /// A user for use in the domain layer for model services.
    /// </summary>
    public class DomainUser
    {
        /// <summary>
        /// UI-friendly name for the current user
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Names of the authorization policies that the user fulfills.
        /// </summary>
        public IEnumerable<string> AuthorizedAs { get; }

        /// <summary>
        /// Construct a new domain user
        /// </summary>
        /// <param name="name">UI-Friendly name for the current user</param>
        /// <param name="authorizedAs">Authorization policies that the user fulfills</param>
        public DomainUser(string name, IEnumerable<string> authorizedAs)
        {
            Name = name;
            AuthorizedAs = authorizedAs;
        }
    }
}
