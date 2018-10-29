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
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// Authorization policies that the user fulfills.
        /// </summary>
        /// <value></value>
        IEnumerable<string> AuthorizedAs { get; }
    }
}
