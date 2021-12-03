namespace VoidCore.Model.Auth;

/// <summary>
/// A singleton to access the current user.
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Get the current domain user.
    /// </summary>
    DomainUser User { get; }
}
