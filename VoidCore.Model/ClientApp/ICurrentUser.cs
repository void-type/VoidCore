namespace VoidCore.Model.ClientApp
{
    /// <summary>
    /// Access the current user's properties.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// UI-friendly name for the current user
        /// </summary>
        /// <value></value>
        string Name { get; }
    }
}
