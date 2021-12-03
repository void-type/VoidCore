using System.Collections.Generic;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Builds CSP header directives.
/// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
/// </summary>
public sealed class CspDirectiveBuilder
{
    private readonly List<string> _sources = new();

    internal CspDirectiveBuilder(string name)
    {
        Name = name;
    }

    internal string Name { get; }
    internal IReadOnlyList<string> Sources => _sources;

    /// <summary>
    /// Allow a source by URI.
    /// </summary>
    /// <param name="source">The source URI</param>
    public CspDirectiveBuilder Allow(string source)
    {
        _sources.Add(source);
        return this;
    }

    /// <summary>
    /// Uses a wildcard to allow any source. Typically unsafe and not recommended.
    /// </summary>
    public CspDirectiveBuilder AllowAny() => Allow("*");

    /// <summary>
    /// Allow no sources.
    /// </summary>
    public CspDirectiveBuilder AllowNone() => Allow("'none'");

    /// <summary>
    /// Allow sources from the origin site.
    /// </summary>
    public CspDirectiveBuilder AllowSelf() => Allow("'self'");

    /// <summary>
    /// Build the directive string value.
    /// </summary>
    /// <returns>The string representation of the header directive.</returns>
    public string Build()
    {
        return $"{Name} {string.Join(" ", _sources)};";
    }
}
