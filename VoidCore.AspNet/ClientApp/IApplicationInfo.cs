namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Information that the ClientApp needs to start.
    /// </summary>
    public interface IApplicationInfo
    {
        /// <summary>
        /// UI-friendly application name to be displayed.
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// The antiforgery token to be sent with every POST request.
        /// </summary>
        string AntiforgeryToken { get; }

        /// <summary>
        /// The header name of the antiforgery token to be sent with every POST request.
        /// </summary>
        string AntiforgeryTokenHeaderName { get; }

        /// <summary>
        /// The UI-friendly user name to be displayed.
        /// </summary>
        string UserName { get; }
    }
}
