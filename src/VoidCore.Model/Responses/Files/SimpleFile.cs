using System;
using System.Collections.Generic;
using VoidCore.Domain;

namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// A view model for sending a file to be downloaded by the client.
    /// </summary>
    public class SimpleFile : ValueObject
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
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if fileName or content is null.</exception>
        public SimpleFile(string fileContent, string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName), "Cannot create a file with null name.");
            }

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
            Content = new FileContent(fileContent);
            Name = fileName;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Content;
        }
    }
}
