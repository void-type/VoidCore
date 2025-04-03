using System.Text;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Responses.Files;

/// <summary>
/// The contents of a file.
/// </summary>
public class FileContent : ValueObject
{
    /// <summary>
    /// Create a new file contents using bytes.
    /// </summary>
    /// <param name="content">The content of the file.</param>
    public FileContent(byte[] content)
    {
        content.EnsureNotNull();
        AsBytes = content;
    }

    /// <summary>
    /// Create a new file contents using a string. Will be decoded from UTF8 to bytes internally.
    /// </summary>
    /// <param name="content">The content of the file.</param>
    public FileContent(string content)
    {
        content.EnsureNotNull();
        AsBytes = Encoding.UTF8.GetBytes(content);
    }

    /// <summary>
    /// Returns the contents of the file in bytes.
    /// </summary>
    public byte[] AsBytes { get; }

    /// <summary>
    /// Returns the contents encoded as a UTF8 string.
    /// </summary>
    public override string ToString() => Encoding.UTF8.GetString(AsBytes);

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return string.Concat(AsBytes.Select(b => $"{b:X2}"));
    }
}
