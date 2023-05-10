using System.IO;
using System.Text.RegularExpressions;

namespace VoidCore.Model.Text;

public static partial class TextHelpers
{
    /// <summary>
    /// Replaces invalid characters in a file name.
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="replacement">Replacement for illegal characters</param>
    public static string GetSafeFileName(this string fileName, string replacement = "_")
    {
        return string.Join(replacement, fileName.Split(Path.GetInvalidFileNameChars()))
            .Replace(@"\", replacement)
            .Replace("..", replacement);
    }

    /// <summary>
    /// Replaces invalid characters in a path to a file. Be sure to check that the path starts with your expected location.
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <param name="replacement">Replacement for illegal characters</param>
    public static string GetSafeFilePath(this string filePath, string replacement = "_")
    {
        return string.Join(replacement, filePath.Split(Path.GetInvalidPathChars()))
            .Replace("..", replacement);
    }

    [GeneratedRegex("[^A-Za-z0-9\\s-]")]
    private static partial Regex NotAlphaNumericSpaceHyphen();

    [GeneratedRegex("\\s+")]
    private static partial Regex MultipleSpaces();

    [GeneratedRegex("\\s")]
    private static partial Regex SingleSpace();

    [GeneratedRegex("-+")]
    private static partial Regex MultipleHyphens();
}
