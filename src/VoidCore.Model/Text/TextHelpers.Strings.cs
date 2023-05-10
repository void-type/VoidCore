using System;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Text;

public static partial class TextHelpers
{
    /// <summary>
    /// Split a string on any newline character.
    /// </summary>
    /// <param name="str">The string to split</param>
    public static string[] SplitOnNewLine(this string str)
    {
        return str
            .EnsureNotNull()
            .Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
    }

    /// <summary>
    /// Indicates whether the specified string is null or an empty string ("").
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <returns>true if the value parameter is null or string.Empty, or if value consists exclusively of white-space characters.</returns>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// Determines whether this string and a specified string object have the same value. Uses the OrdinalIgnoreCase rule set to perform a case-insensitive and culture-agnostic comparison.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <param name="value">The string to compare with</param>
    /// <returns>true if the value of the value parameter is the same as this string; otherwise, false.</returns>
    public static bool EqualsIgnoreCase(this string str, string value)
    {
        return str.Equals(value, StringComparison.OrdinalIgnoreCase);
    }
}
