﻿namespace VoidCore.AspNet.Security;

/// <summary>
/// Extensions to wrap directive builders with methods that ease building directives to the specification.
/// </summary>
public static class CspDirectiveBuilderExtensions
{
    /// <summary>
    /// Enhance a CspDirectiveBuilder with methods that support "src" directives.
    /// </summary>
    /// <param name="cspDirectiveBuilder">The internal directive builder.</param>
    /// <param name="nonce">The nonce generated by the server</param>
    public static CspSourceDirectiveBuilder ForSources(this CspDirectiveBuilder cspDirectiveBuilder, string nonce)
    {
        return new CspSourceDirectiveBuilder(cspDirectiveBuilder, nonce);
    }
}
