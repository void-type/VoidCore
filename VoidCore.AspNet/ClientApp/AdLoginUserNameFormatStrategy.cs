using System;
using System.Linq;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Get the samAccountName from a fully-qualified Active Directory login.
    /// </summary>
    public class AdLoginUserNameFormatStrategy : IUserNameFormatStrategy
    {
        /// <summary>
        /// Get the user name from a fully-qualified AD login.
        /// Eg: DOMAIN1\UserName returns UserName
        /// </summary>
        /// <param name="adLogin"></param>
        /// <returns></returns>
        public string Format(string adLogin)
        {
            var lastLoginPart = adLogin?
                .Split(new [] { "\\" }, StringSplitOptions.None)
                .LastOrDefault();

            return string.IsNullOrWhiteSpace(lastLoginPart) ? "Unknown" : lastLoginPart;
        }
    }
}
