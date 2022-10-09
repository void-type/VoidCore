using System.Collections.Generic;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Responses.Files;

/// <summary>
/// A view model for sending a file to be downloaded by the client.
/// </summary>
public class SimpleFile : ValueObject
{
    /// <summary>
    /// Create a new file from a byte array. Useful for binary files.
    /// </summary>
    /// <param name="content">The byte representation of the file contents</param>
    /// <param name="name">The name of the file</param>
    public SimpleFile(string content, string name)
    {
        Name = name.EnsureNotNullOrEmpty();
        Content = new FileContent(content);
    }

    /// <summary>
    /// Create a new file from string content. Useful for human-readable text files. Uses UTF8 encoding.
    /// </summary>
    /// <param name="content">The string representation of the file contents</param>
    /// <param name="name">The name of the file</param>
    public SimpleFile(byte[] content, string name)
    {
        Name = name.EnsureNotNullOrEmpty();
        Content = new FileContent(content);
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
    public string Name { get; }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Content;
    }
}
