using System.Collections.Generic;
using VoidCore.Domain;
using VoidCore.Domain.Guards;

namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// A view model for sending a file to be downloaded by the client.
    /// </summary>
    public class SimpleFile : ValueObject
    {
        private string _name;

        /// <summary>
        /// Create a new file from a byte array. Useful for binary files.
        /// </summary>
        /// <param name="fileContent">The byte representation of the file contents</param>
        /// <param name="fileName">The name of the file</param>
        public SimpleFile(string fileContent, string fileName)
        {
            Name = fileName;
            Content = new FileContent(fileContent);
        }

        /// <summary>
        /// Create a new file from string content. Useful for human-readable text files. Uses UTF8 encoding.
        /// </summary>
        /// <param name="fileContent">The string representation of the file contents</param>
        /// <param name="fileName">The name of the file</param>
        public SimpleFile(byte[] fileContent, string fileName)
        {
            Name = fileName;
            Content = new FileContent(fileContent);
        }

        /// <summary>
        /// The content of the file.
        /// </summary>
        /// <value></value>
        public FileContent Content { get; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get => _name;

            private set => _name = value.EnsureNotNullOrEmpty(nameof(Name));
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Content;
        }
    }
}
