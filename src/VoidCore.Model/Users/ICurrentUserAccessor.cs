using System.Collections.Generic;

namespace VoidCore.Model.Users
{
    /// <summary>
    /// Access the current user's properties.
    /// </summary>
    public interface ICurrentUserAccessor
    {
        /// <summary>
        /// Get the current domain user.
        /// </summary>
        DomainUser User { get; }
    }
}
