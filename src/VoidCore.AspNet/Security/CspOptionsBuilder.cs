using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Builds the options to configure the CSP header.
    /// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
    /// </summary>
    public sealed class CspOptionsBuilder
    {
        private List<CspDirectiveBuilder> _directives = new List<CspDirectiveBuilder>();

        internal CspOptionsBuilder() { }

        /// <summary>
        /// Set the default fallback policy for sources that don't match any other set policy.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Defaults => Custom("default-src");

        /// <summary>
        /// Set the policy for script sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Scripts => Custom("script-src");

        /// <summary>
        /// Set the policy for style sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Styles => Custom("style-src");

        /// <summary>
        /// Set the policy for img sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Images => Custom("img-src");

        /// <summary>
        /// Set the policy for font sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Fonts => Custom("font-src");

        /// <summary>
        /// Set the policy for media sources like video and audio.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Media => Custom("media-src");

        private CspDirectiveBuilder _reportUri => Custom("report-uri");
        private bool _isReportOnly;

        /// <summary>
        /// Set the header to report-only. Content will not be blocked.
        /// </summary>
        /// <param name="reportUri">The optional URI to send JSON reports to of content that would be normally blocked.</param>
        /// <returns>The builder for chaining.</returns>
        public CspOptionsBuilder ReportOnly(string reportUri = null)
        {
            _isReportOnly = true;

            return string.IsNullOrWhiteSpace(reportUri) ? this : SetReportUri(reportUri);
        }

        /// <summary>
        /// Set a URI to send blocked content JSON reports to. These reports can be collected to track violations on client pages.
        /// </summary>
        /// <param name="reportUri">The URI that collects CSP violation reports.</param>
        /// <returns>The builder for chaining.</returns>
        public CspOptionsBuilder SetReportUri(string reportUri)
        {
            if (_reportUri.Sources.Count > 0)
            {
                throw new InvalidOperationException("Cannot set more than one report-uri directive value.");
            }

            _reportUri.Allow(reportUri);
            return this;
        }

        /// <summary>
        /// Set a custom directive that is not listed as a property of this builder.
        /// </summary>
        /// <param name="directiveName">The name of the custom directive.</param>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Custom(string directiveName)
        {
            Maybe<CspDirectiveBuilder> maybeBuilder = _directives.FirstOrDefault(d => d.Name == directiveName);

            return maybeBuilder.Unwrap(() =>
            {
                var newBuilder = new CspDirectiveBuilder(directiveName);
                _directives.Add(newBuilder);
                return newBuilder;
            });
        }

        internal CspOptions Build()
        {
            return new CspOptions(_isReportOnly, _directives);
        }
    }
}
