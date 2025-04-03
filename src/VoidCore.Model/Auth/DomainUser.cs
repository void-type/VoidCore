namespace VoidCore.Model.Auth;

/// <summary>
/// A user for use in the domain layer and model services.
/// </summary>
public class DomainUser
{
    /// <summary>
    /// Construct a new domain user
    /// </summary>
    /// <param name="login">UI-Friendly name for the current user</param>
    /// <param name="authorizedAs">Authorization policies that the user fulfills</param>
    public DomainUser(string login, IEnumerable<string> authorizedAs)
    {
        Login = login;
        AuthorizedAs = authorizedAs;
    }

    /// <summary>
    /// UI-friendly name for the current user
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Names of the authorization policies that the user fulfills.
    /// </summary>
    public IEnumerable<string> AuthorizedAs { get; }
}
