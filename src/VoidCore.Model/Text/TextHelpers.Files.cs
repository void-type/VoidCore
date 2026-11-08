using System.Text.RegularExpressions;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Text;

public static partial class TextHelpers
{
    // Characters that are invalid in a file name on at least one platform, so a name is safe to move between Windows and Linux.
    private static readonly char[] s_invalidFileNameChars = Path.GetInvalidFileNameChars()
        .Concat(Enumerable.Range(0, 32).Select(i => (char)i))
        .Concat("<>:\"/\\|?*")
        .Distinct()
        .ToArray();

    private static readonly HashSet<string> s_reservedFileNames = new(
        new[] { "CON", "PRN", "AUX", "NUL", "CONIN$", "CONOUT$" }
            .Concat(Enumerable.Range(0, 10).SelectMany(i => new[] { $"COM{i}", $"LPT{i}" })),
        StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Makes a string safe to use as a single file name on Windows and Linux. Replaces characters that are invalid in a file name
    /// (including path separators and ':'), "..", and trailing dots and spaces. Names that Windows reserves for devices (CON, NUL,
    /// COM1, and so on, with or without an extension) are prefixed with the replacement. A name that would otherwise be empty
    /// becomes the replacement. This doesn't make a path safe: use it on the file name only, then combine it with a trusted folder.
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="replacement">Replacement for illegal characters. Can't contain illegal characters or "..".</param>
    /// <exception cref="ArgumentException">The replacement is invalid</exception>
    public static string GetSafeFileName(this string fileName, string replacement = "_")
    {
        fileName.EnsureNotNull();
        replacement.EnsureNotNull();

        if (replacement.IndexOfAny(s_invalidFileNameChars) >= 0 || replacement.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException("The replacement can't contain characters that are invalid in a file name or \"..\".", nameof(replacement));
        }

        var safeName = RemoveRepeatedDots(string.Join(replacement, fileName.Split(s_invalidFileNameChars)), replacement)
            .Trim()
            .TrimEnd('.', ' ');

        var fallback = replacement.Length > 0 ? replacement : "_";

        if (safeName.Length == 0)
        {
            return fallback;
        }

        var stem = safeName.Split('.')[0].TrimEnd();

        return s_reservedFileNames.Contains(stem)
            ? fallback + safeName
            : safeName;
    }

    /// <summary>
    /// Replaces invalid characters and ".." in a path to a file. This is not a sandbox: absolute and rooted paths are left intact, so
    /// always check that the path starts with your expected location, and use <see cref="GetSafeFileName"/> on user-supplied file names
    /// before combining them with a folder.
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <param name="replacement">Replacement for illegal characters. Can't contain "..".</param>
    /// <exception cref="ArgumentException">The replacement is invalid</exception>
    public static string GetSafeFilePath(this string filePath, string replacement = "_")
    {
        filePath.EnsureNotNull();
        replacement.EnsureNotNull();

        if (replacement.Contains("..", StringComparison.Ordinal) || replacement.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException("The replacement can't contain characters that are invalid in a path or \"..\".", nameof(replacement));
        }

        return RemoveRepeatedDots(string.Join(replacement, filePath.Split(Path.GetInvalidPathChars())), replacement);
    }

    // Replacing ".." once can leave a new one behind, for example "...." or ". ." around a replacement of ".".
    private static string RemoveRepeatedDots(string value, string replacement)
    {
        while (value.Contains("..", StringComparison.Ordinal))
        {
            value = value.Replace("..", replacement, StringComparison.Ordinal);
        }

        return value;
    }
    private const int RegexTimeoutMilliseconds = 1000;

    [GeneratedRegex("[^A-Za-z0-9\\s-]", RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex NotAlphaNumericSpaceHyphen();

    [GeneratedRegex("\\s+", RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex MultipleSpaces();

    [GeneratedRegex("\\s", RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex SingleSpace();

    [GeneratedRegex("-+", RegexOptions.None, RegexTimeoutMilliseconds)]
    private static partial Regex MultipleHyphens();
}
