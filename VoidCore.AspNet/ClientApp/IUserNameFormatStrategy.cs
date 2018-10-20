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
        /// <param name="fullUserName"></param>
        /// <returns>The UI-friendly user name</returns>
        string Format(string fullUserName);
    }
}
