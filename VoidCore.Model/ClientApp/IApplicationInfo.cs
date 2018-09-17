using System.Collections.Generic;

namespace VoidCore.Model.ClientApp
{
    /// <summary>
    /// Information to start the client application
    /// </summary>
    public interface IApplicationInfo
    {
        /// <summary>
        /// The UI-friendly name of the application
        /// </summary>
        /// <value></value>
        string ApplicationName { get; }

        /// <summary>
        /// The value of the header antiforgery token
        /// </summary>
        /// <value></value>
        string AntiforgeryToken { get; }

        /// <summary>
        /// The header name of the antiforgery token
        /// </summary>
        /// <value></value>
        string AntiforgeryTokenHeaderName { get; }

        /// <summary>
        /// The UI-friendly user name
        /// </summary>
        /// <value></value>
        string UserName { get; }

        /// <summary>
        /// A list of authorization policies that the user fulfills.
        /// </summary>
        /// <value></value>
        IEnumerable<string> UserPolicies { get; }
    }
}