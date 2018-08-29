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
        string ApplicationName { get; set; }

        /// <summary>
        /// The antiforgery token to be sent with every POST request.
        /// </summary>
        string AntiforgeryToken { get; set; }

        /// <summary>
        /// The header name of the antiforgery token to be sent with every POST request.
        /// </summary>
        string AntiforgeryTokenHeaderName { get; set; }

        /// <summary>
        /// The UI-friendly user name to be displayed.
        /// </summary>
        string UserName { get; set; }
    }
}
