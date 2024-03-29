﻿using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

/// <summary>
/// A header for adding Content Security Policy to a webpage.
/// </summary>
public class CspHeader
{
    /// <summary>
    /// Construct a new CspHeader
    /// </summary>
    /// <param name="options">The CspOptions to configure the header with</param>
    public CspHeader(CspOptions options)
    {
        options.EnsureNotNull();

        Key = $"Content-Security-Policy{(options.IsReportOnly ? "-Report-Only" : string.Empty)}";
        Value = string.Concat(options.Directives);
    }

    /// <summary>
    /// The header key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// The header value.
    /// </summary>
    public string Value { get; }
}
