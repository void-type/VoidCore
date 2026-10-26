namespace VoidCore.Model.Auth;

/// <summary>
/// A singleton to access the current user.
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Get the current domain user, including the policies they are authorized for.
    /// </summary>
    Task<DomainUser> GetUser();

    /// <summary>
    /// Get the current user's name without evaluating authorization policies.
    /// This is synchronous and cheap, so it is safe to call from places that can't await, like EF Core's SaveChanges.
    /// </summary>
    string GetUserName();
}
