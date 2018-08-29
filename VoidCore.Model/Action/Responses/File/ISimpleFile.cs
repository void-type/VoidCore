namespace VoidCore.Model.Action.Responses.File
{
    /// <summary>
    /// A view model for sending a file to be downloaded by the client.
    /// </summary>
    public interface ISimpleFile
    {
        /// <summary>
        /// The content bytes of the file.
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// The name to save the file as on the client.
        /// </summary>
        /// <value></value>
        string Name { get; }
    }
}
