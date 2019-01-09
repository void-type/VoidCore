using System.Collections.Generic;
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
        public FileContent(byte[] content)
        {
            _content = content;
        }

        /// <summary>
        /// Create a new file contents using a string. Will be decoded from UTF8 to bytes internally.
        /// </summary>
        /// <param name="content">The content of the file.</param>
        public FileContent(string content)
        {
            _content = Encoding.UTF8.GetBytes(content);
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _content;
        }

        private byte[] _content;
    }
}
