using System.Net;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Text;

public static partial class TextHelpers
{
    private static readonly string[] s_safeLinkSchemes = ["http", "https", "mailto", "tel"];

    /// <summary>
    /// Make an anchor tag string. The caption and URL are HTML-encoded, so they can't inject markup or attributes.
    /// </summary>
    /// <param name="caption">The text shown to the user. It is HTML-encoded.</param>
    /// <param name="urlSegments">A series of url segments to be joined with "/"</param>
    /// <exception cref="ArgumentException">The URL has a scheme other than http, https, mailto, or tel (for example "javascript:")</exception>
    public static string Link(string caption, params string[] urlSegments)
    {
        caption.EnsureNotNull();
        urlSegments.EnsureNotNull();

        var url = string.Join("/", urlSegments);

        if (!HasSafeLinkScheme(url))
        {
            throw new ArgumentException("The URL must be relative or use the http, https, mailto, or tel scheme.", nameof(urlSegments));
        }

        return $"<a href=\"{WebUtility.HtmlEncode(url)}\">{WebUtility.HtmlEncode(caption)}</a>";
    }

    private static bool HasSafeLinkScheme(string url)
    {
        // Browsers ignore whitespace and control characters inside a scheme, so "java\tscript:" still runs.
        var normalized = new string(url.Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c)).ToArray());

        var colon = normalized.IndexOf(':', StringComparison.Ordinal);
        var firstDelimiter = normalized.IndexOfAny(['/', '?', '#']);

        if (colon < 0 || (firstDelimiter >= 0 && firstDelimiter < colon))
        {
            return true;
        }

        return s_safeLinkSchemes.Contains(normalized[..colon], StringComparer.OrdinalIgnoreCase);
    }
}
