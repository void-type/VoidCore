using System.Linq;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Formatter for getting a UI-friendly user name.
    /// </summary>
    public interface IUserNameFormatter
    {
        /// <summary>
        /// Get the UI-friendly user name from a longer string.
        /// </summary>
        /// <param name="fullUserName"></param>
        /// <returns></returns>
        string Format(string fullUserName);
    }

    /// <summary>
    /// Get the samAccountName from a fully-qualified AD login.
    /// </summary>
    public class AdLoginUserNameFormatter : IUserNameFormatter
    {
        /// <summary>
        /// Get the user name from a fully-qualified AD login.
        /// Eg: DOMAIN1\UserName returns UserName
        /// </summary>
        /// <param name="fullUserName"></param>
        /// <returns></returns>
        public string Format(string fullUserName)
        {
            return fullUserName?.Split("\\").LastOrDefault() ?? "Unknown";
        }
    }
}
