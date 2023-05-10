using System;
using System.Globalization;
using System.Linq;
using System.Text;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Text;

public static partial class TextHelpers
{
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

        var output = phrase.RemoveAccents().ToLowerInvariant();

        // Remove all special characters
        output = NotAlphaNumericSpaceHyphen().Replace(output, "");

        // Replace repeat spaces with a single space
        output = MultipleSpaces().Replace(output, " ").Trim();

        // Replace all spaces with hyphen
        output = SingleSpace().Replace(output, "-");

        // Replace repeat hyphens with a single hyphen
        output = MultipleHyphens().Replace(output, "-");

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

        return output;
    }

    /// <summary>
    /// Removes all accents from the input string.
    /// </summary>
    /// <param name="text">The input string.</param>
    private static string RemoveAccents(this string text)
    {
        text = text.Normalize(NormalizationForm.FormD);

        // Remove modifier characters
        var chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}
