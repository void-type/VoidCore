using System.Collections.Generic;

namespace VoidCore.Model.ClientApp
{
    /// <summary>
    /// Access the current user's properties.
    /// </summary>
    public interface ICurrentUserAccessor
    {
        /// <summary>
        /// UI-friendly name for the current user
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Names of the authorization policies that the user fulfills.
        /// </summary>
        IEnumerable<string> AuthorizedAs { get; }
    }
}
