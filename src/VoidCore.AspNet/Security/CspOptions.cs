using System.Collections.Generic;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Options for configuring the CSP header.
    /// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
    /// </summary>
    public sealed class CspOptions
    {
        internal CspOptions(bool isReportOnly, IReadOnlyList<CspDirectiveBuilder> directives)
        {
            IsReportOnly = isReportOnly;
            DirectiveBuilders = directives;
        }

        internal bool IsReportOnly { get; }
        internal IReadOnlyList<CspDirectiveBuilder> DirectiveBuilders { get; }
    }
}
