using System.Linq;

namespace VoidCore.Model.Auth;

/// <summary>
/// Get the samAccountName from an email address.
/// </summary>
public class EmailUserNameFormatStrategy : IUserNameFormatStrategy
{
    /// <summary>
    /// Get the user name from a fully-qualified AD login.
    /// Eg: UserName@Contoso.com returns UserName
    /// </summary>
    /// <param name="fullUserName">A fully-qualified AD login like UserName@Contoso.com</param>
    public string Format(string? fullUserName)
    {
        var userName = fullUserName?.Split('@').FirstOrDefault();

        return string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName;
    }
}
