using System.Text;

namespace VoidCore.Model.Responses.Files
{
    /// <inheritdoc/>
    public class SimpleFile : ISimpleFile
    {
        /// <summary>
        /// Create a new file from a byte array. Useful for binary files.
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        public SimpleFile(byte[] fileContent, string fileName)
        {
            Content = fileContent;
            Name = fileName;
        }

        /// <summary>
        /// Create a new file from string content. Useful for human-readable text files.
        /// Uses UTF8 encoding.
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        public SimpleFile(string fileContent, string fileName)
        {
            Content = Encoding.UTF8.GetBytes(fileContent);
            Name = fileName;
        }

        /// <inheritdoc/>
        public byte[] Content { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string ContentAsString => Encoding.UTF8.GetString(Content);
    }
}
