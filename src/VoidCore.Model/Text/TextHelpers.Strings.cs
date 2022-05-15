using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            .EnsureNotNull(nameof(str))
            .Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
    }

    /// <summary>
    /// Turn a string, such as an article title, into a slug.
    /// Removes all accents, special characters, additional spaces.
    /// Replaces spaces with hyphens and lowercases it.
    /// Adapted from https://www.c-sharpcorner.com/blogs/make-url-slugs-in-asp-net-using-c-sharp2
    /// </summary>
    /// <param name="phrase">The original string</param>
    /// <param name="maxLength">Max number of characters for the final slug</param>
    /// <param name="splitOnWord">When true, partial words after truncation will be removed</param>
    public static string Slugify(this string phrase, int? maxLength = null, bool splitOnWord = false)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            return string.Empty;
        }

        // Remove all accents and make the string lower case.
        var output = phrase.RemoveAccents().ToLower();

        // Remove all special characters from the string.
        output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

        // Remove all additional spaces in favor of just one.
        output = Regex.Replace(output, @"\s+", " ").Trim();

        // Replace all spaces with the hyphen.
        output = Regex.Replace(output, @"\s", "-");

        // Shorten if needed
        if (maxLength.HasValue && output.Length > maxLength)
        {
            var fullLengthOutput = output;

            // Shorten to max length then remove trailing hyphen if any.
            output = output[..maxLength.Value].TrimEnd('-');

            if (splitOnWord)
            {
                // Check to see if the character after truncation is a hyphen
                var isLastWordIncomplete = fullLengthOutput[output.Length] != '-';

                if (isLastWordIncomplete)
                {
                    var lastHyphen = Math.Max(output.LastIndexOf('-'), 0);
                    output = output[..lastHyphen];
                }
            }
        }

        // Return the slug.
        return output;
    }

    /// <summary>
    /// Removes all accents from the input string.
    /// </summary>
    /// <param name="text">The input string.</param>
    private static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        text = text.Normalize(NormalizationForm.FormD);

        // Remove modifier characters
        var chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}
