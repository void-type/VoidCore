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
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Returns a default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <param name="defaultValue">Default value to return</param>
    /// <returns>Returns a default value if the string is null or empty, otherwise returns the string.</returns>
    public static string DefaultIfNullOrEmpty(this string? str, string defaultValue)
    {
        return !string.IsNullOrEmpty(str) ? str : defaultValue;
    }

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <returns>true if the value parameter is null or string.Empty, or if value consists exclusively of white-space characters.</returns>
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// Returns a default value if the string is null or whitespace.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <param name="defaultValue">Default value to return</param>
    /// <returns>Returns a default value if the string is null or whitespace, otherwise returns the string.</returns>
    public static string DefaultIfNullOrWhiteSpace(this string? str, string defaultValue)
    {
        return !string.IsNullOrWhiteSpace(str) ? str : defaultValue;
    }

    /// <summary>
    /// Determines whether this string and a specified string object have the same value. Uses the OrdinalIgnoreCase rule set to perform a case-insensitive and culture-agnostic comparison.
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <param name="value">The string to compare with</param>
    /// <returns>true if the value of the value parameter is the same as this string; otherwise, false.</returns>
    public static bool EqualsIgnoreCase(this string? str, string? value)
    {
        return string.Equals(str, value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks a series of strings and returns the first that's not null or whitespace.
    /// </summary>
    /// <param name="values">A series of strings to check</param>
    /// <returns>A string from the series, or string.Empty if none match.</returns>
    public static string FirstNotNullOrWhiteSpace(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }
}
