using System.Linq;

namespace VoidCore.Model.Auth;

/// <summary>
/// Get the samAccountName from a fully-qualified Active Directory login.
/// </summary>
public class AdLoginUserNameFormatStrategy : IUserNameFormatStrategy
{
    /// <summary>
    /// Get the user name from a fully-qualified AD login.
    /// Eg: DOMAIN1\UserName returns UserName
    /// </summary>
    /// <param name="fullUserName">A fully-qualified AD login like DOMAIN1\UserName</param>
    public string Format(string? fullUserName)
    {
        var lastLoginPart = fullUserName?.Split('\\').LastOrDefault();

        return string.IsNullOrWhiteSpace(lastLoginPart) ? "Unknown" : lastLoginPart;
    }
}
