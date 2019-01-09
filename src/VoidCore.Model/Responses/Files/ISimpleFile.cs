namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// A view model for sending a file to be downloaded by the client.
    /// </summary>
    public interface ISimpleFile
    {
        /// <summary>
        /// The contents file.
        /// </summary>
        FileContent Content { get; }

        /// <summary>
        /// The name to save the file as on the client.
        /// </summary>
        string Name { get; }
    }
}
