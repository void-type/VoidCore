namespace VoidCore.Model.Configuration
{
    /// <summary>
    /// Variables that can be used to make the domain layer aware of it's environment and it's URL.
    /// </summary>
    public interface IWebAppVariables
    {
        /// <summary>
        /// The user-friendly application name.
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// The base URL of the application or the root of the website.
        /// </summary>
        /// <value></value>
        string BaseUrl { get; }

        /// <summary>
        /// The name of the current environment, such as "Production" or "Staging."
        /// </summary>
        /// <value></value>
        string Environment { get; }
    }
}
