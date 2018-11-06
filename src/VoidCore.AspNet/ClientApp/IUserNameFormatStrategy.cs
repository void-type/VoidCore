namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Formatter for getting a UI-friendly user name.
    /// </summary>
    public interface IUserNameFormatStrategy
    {
        /// <summary>
        /// Get the UI-friendly user name from a longer string.
        /// </summary>
        /// <param name="fullUserName">The full or non-UI-friendly user name.</param>
        /// <returns>The UI-friendly user name</returns>
        string Format(string fullUserName);
    }
}
