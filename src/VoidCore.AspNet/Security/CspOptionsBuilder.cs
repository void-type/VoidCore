﻿using System;
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
        private readonly List<CspDirectiveBuilder> _directiveBuilders = new();
        private bool _isReportOnly;

        /// <summary>
        /// Set the policy for base-uri. This restricts the base tag to the specified values.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder BaseUri => Custom("base-uri");

        /// <summary>
        /// Set the default fallback policy for sources that don't match any other set policy.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder DefaultSources => Custom("default-src").ForSources();

        /// <summary>
        /// Set the policy for font sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder FontSources => Custom("font-src").ForSources();

        /// <summary>
        /// Set the policy for frame-ancestors.
        /// This specifies valid parents that can embed this page in a frame, iframe, object, embed, and applet tags.
        /// Setting to none is similar to x-frame-options: deny.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder FrameAncestors => Custom("frame-ancestors");

        /// <summary>
        /// Set the policy for img sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder ImageSources => Custom("img-src").ForSources();

        /// <summary>
        /// Set the policy for media sources like video and audio.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder MediaSources => Custom("media-src").ForSources();

        /// <summary>
        /// Set the policy for object sources. These include object, embed, and applet tags.
        /// It is strongly recommended to set this to 'none'.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder ObjectSources => Custom("object-src").ForSources();

        /// <summary>
        /// Set the policy for script sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder ScriptSources => Custom("script-src").ForSources();

        /// <summary>
        /// Set the policy for stylesheet sources.
        /// </summary>
        /// <returns>The builder for chaining.</returns>
        public CspSourceDirectiveBuilder StyleSources => Custom("style-src").ForSources();

        private CspDirectiveBuilder ReportUri => Custom("report-uri");

        /// <summary>
        /// Set a custom directive that is not listed as a property of this builder.
        /// </summary>
        /// <param name="directiveName">The name of the custom directive.</param>
        /// <returns>The builder for chaining.</returns>
        public CspDirectiveBuilder Custom(string directiveName)
        {
            Maybe<CspDirectiveBuilder> maybeBuilder = _directiveBuilders.FirstOrDefault(d => d.Name == directiveName);

            return maybeBuilder.Unwrap(() =>
            {
                var newBuilder = new CspDirectiveBuilder(directiveName);
                _directiveBuilders.Add(newBuilder);
                return newBuilder;
            });
        }

        /// <summary>
        /// Set the header to report-only. Content will not be blocked.
        /// Report-uri is deprecated, but the report-to directive is not yet supported in most browsers.
        /// </summary>
        /// <param name="reportUri">The optional URI to send JSON reports to of content that would be normally blocked.</param>
        /// <returns>The builder for chaining.</returns>
        public CspOptionsBuilder ReportOnly(string? reportUri = null)
        {
            _isReportOnly = true;

            return string.IsNullOrWhiteSpace(reportUri) ? this : SetReportUri(reportUri);
        }

        /// <summary>
        /// Set a URI to send blocked content JSON reports to. These reports can be collected to track violations on
        /// client pages.
        /// Report-uri is deprecated, but the report-to directive is not yet supported in most browsers.
        /// </summary>
        /// <param name="reportUri">The URI that collects CSP violation reports.</param>
        /// <returns>The builder for chaining.</returns>
        /// <exception cref="InvalidOperationException">Throws if report-uri already set.</exception>
        public CspOptionsBuilder SetReportUri(string reportUri)
        {
            if (ReportUri.Sources.Count > 0)
            {
                throw new InvalidOperationException("Cannot set more than one report-uri directive value.");
            }

            ReportUri.Allow(reportUri);
            return this;
        }

        /// <summary>
        /// Build the CspOptions as configured by this builder.
        /// </summary>
        /// <returns>A new CspOptions</returns>
        public CspOptions Build()
        {
            var directives = new List<string>(_directiveBuilders.Select(d => d.Build()));

            return new CspOptions(_isReportOnly, directives);
        }
    }
}
