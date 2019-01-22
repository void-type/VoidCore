using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidCore.Domain;

namespace VoidCore.Model.Responses.Files
{
    /// <summary>
    /// The contents of a file.
    /// </summary>
    public class FileContent : ValueObject
    {
        /// <summary>
        /// Returns the contents encoded as a UTF8 string.
        /// </summary>
        /// <returns></returns>
        public string AsString => Encoding.UTF8.GetString(_content);

        /// <summary>
        /// Returns the contents of the file in bytes.
        /// </summary>
        public byte[] AsBytes => _content;

        /// <summary>
        /// Create a new file contents using bytes.
        /// </summary>
        /// <param name="content">The content of the file.</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if content is null.</exception>
        public FileContent(byte[] content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "Cannot create a file content with null content.");
            }

            _content = content;
        }

        /// <summary>
        /// Create a new file contents using a string. Will be decoded from UTF8 to bytes internally.
        /// </summary>
        /// <param name="content">The content of the file.</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if content is null.</exception>
        public FileContent(string content) : this(Encoding.UTF8.GetBytes(content)) { }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return string.Join("", _content.Select(b => string.Format("{0:X2}", b)));
        }

        private byte[] _content;
    }
}
