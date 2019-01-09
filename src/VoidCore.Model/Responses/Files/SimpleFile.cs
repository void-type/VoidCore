using System.Text;

namespace VoidCore.Model.Responses.Files
{
    /// <inheritdoc/>
    public class SimpleFile : ISimpleFile
    {
        /// <summary>
        /// The content of the file.
        /// </summary>
        /// <value></value>
        public FileContent Content { get; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// Create a new file from a byte array. Useful for binary files.
        /// </summary>
        /// <param name="fileContent">The byte representation of the file contents</param>
        /// <param name="fileName">The name of the file</param>
        public SimpleFile(string fileContent, string fileName)
        {
            Content = new FileContent(fileContent);
            Name = fileName;
        }

        /// <summary>
        /// Create a new file from string content. Useful for human-readable text files.
        /// Uses UTF8 encoding.
        /// </summary>
        /// <param name="fileContent">The string representation of the file contents</param>
        /// <param name="fileName">The name of the file</param>
        public SimpleFile(byte[] fileContent, string fileName)
        {
            Content = new FileContent(fileContent);
            Name = fileName;
        }
    }
}
