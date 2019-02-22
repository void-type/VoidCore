using System.Linq;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Get the samAccountName from a fully-qualified Active Directory login.
    /// </summary>
    public class EmailUserNameFormatStrategy : IUserNameFormatStrategy
    {
        /// <summary>
        /// Get the user name from a fully-qualified AD login.
        /// Eg: UserName@Contoso.com returns UserName
        /// </summary>
        /// <param name="adLogin">A fully-qualified AD login like DOMAIN1\UserName</param>
        public string Format(string adLogin)
        {
            var userName = adLogin?.Split("@").FirstOrDefault();

            return string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName;
        }
    }
}
