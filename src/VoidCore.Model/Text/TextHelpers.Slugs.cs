using System.Globalization;
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
    /// <param name="splitOnWord">When true, partial words after truncation will be removed. A single word longer than the max length is cut instead.</param>
    /// <exception cref="ArgumentOutOfRangeException">The max length is negative</exception>
    public static string Slugify(this string? phrase, int? maxLength = null, bool splitOnWord = false)
    {
        if (maxLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "The max length can't be negative.");
        }

        if (string.IsNullOrWhiteSpace(phrase))
        {
            return string.Empty;
        }

        var output = Transliterate(phrase.RemoveAccents().ToLowerInvariant());

        // Remove all special characters
        output = NotAlphaNumericSpaceHyphen().Replace(output, "");

        // Replace repeat spaces with a single space
        output = MultipleSpaces().Replace(output, " ").Trim();

        // Replace all spaces with hyphen
        output = SingleSpace().Replace(output, "-");

        // Replace repeat hyphens with a single hyphen, and remove them from the ends
        output = MultipleHyphens().Replace(output, "-").Trim('-');

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
                    // A single long word has no word break to cut at, so keep the hard cut.
                    var lastHyphen = output.LastIndexOf('-');
                    output = lastHyphen > 0 ? output[..lastHyphen] : output;
                }
            }
        }

        return output;
    }

    // Letters that don't decompose into a base letter and an accent, so RemoveAccents leaves them for the special character filter to drop.
    private static string Transliterate(string text)
    {
        return text
            .Replace("ß", "ss", StringComparison.Ordinal)
            .Replace("æ", "ae", StringComparison.Ordinal)
            .Replace("œ", "oe", StringComparison.Ordinal)
            .Replace("ø", "o", StringComparison.Ordinal)
            .Replace("ł", "l", StringComparison.Ordinal)
            .Replace("đ", "d", StringComparison.Ordinal)
            .Replace("ð", "d", StringComparison.Ordinal)
            .Replace("þ", "th", StringComparison.Ordinal)
            .Replace("ħ", "h", StringComparison.Ordinal)
            .Replace("ı", "i", StringComparison.Ordinal);
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
