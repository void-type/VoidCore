namespace VoidCore.AspNet.Authorization
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
}
