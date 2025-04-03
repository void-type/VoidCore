using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Builds the options to configure the CSP header.
/// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
/// </summary>
public sealed class CspOptionsBuilder
{
    private readonly List<CspDirectiveBuilder> _directiveBuilders = [];
    private readonly string _nonce;
    private bool _isReportOnly;

    /// <summary>
    /// Construct a new builder.
    /// </summary>
    /// <param name="nonce">A nonce to use in header directives</param>
    public CspOptionsBuilder(string nonce)
    {
        _nonce = nonce.EnsureNotNullOrEmpty();
    }

    /// <summary>
    /// Set the policy for base-uri. This restricts the base tag to the specified values.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder BaseUri => Custom("base-uri");

    /// <summary>
    /// Set the default fallback policy for sources that don't match any other set policy.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder DefaultSources => Custom("default-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for connect sources. Controls which network locations a page can connect to
    /// via fetch, WebSockets, EventSource, XMLHttpRequest, etc.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ConnectSources => Custom("connect-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for frame sources. Controls which URLs can be loaded using frame and iframe elements.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder FrameSources => Custom("frame-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for frame-ancestors.
    /// This specifies valid parents that can embed this page in a frame, iframe, object, embed, and applet tags.
    /// Setting to none is similar to x-frame-options: deny.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder FrameAncestors => Custom("frame-ancestors");

    /// <summary>
    /// Set the policy for script sources.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ScriptSources => Custom("script-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for script element sources. Controls which URLs can be loaded as JavaScript
    /// using script elements. Falls back to script-src if not specified.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ScriptElementSources => Custom("script-src-elem").ForSources(_nonce);

    /// <summary>
    /// Set the policy for script attribute sources. Controls which inline event handlers can be executed
    /// (like onclick, onload). Falls back to script-src if not specified.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ScriptAttributeSources => Custom("script-src-attr").ForSources(_nonce);

    /// <summary>
    /// Set the policy for style sources.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder StyleSources => Custom("style-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for style element sources. Controls which URLs can be loaded as CSS
    /// using style elements and link elements with rel="stylesheet". Falls back to style-src if not specified.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder StyleElementSources => Custom("style-src-elem").ForSources(_nonce);

    /// <summary>
    /// Set the policy for style attribute sources. Controls which inline style attributes can be applied.
    /// Falls back to style-src if not specified.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder StyleAttributeSources => Custom("style-src-attr").ForSources(_nonce);

    /// <summary>
    /// Set the policy for font sources.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder FontSources => Custom("font-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for img sources.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ImageSources => Custom("img-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for media sources like video and audio.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder MediaSources => Custom("media-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for object sources. These include object, embed, and applet tags.
    /// It is strongly recommended to set this to 'none'.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ObjectSources => Custom("object-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for manifest sources. Controls which manifest files can be applied to the resource.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ManifestSources => Custom("manifest-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for worker sources. Controls which URLs can be loaded as Worker, SharedWorker, or ServiceWorker.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder WorkerSources => Custom("worker-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for child sources. Controls which URLs can be loaded using frame and worker elements.
    /// Note: This directive is somewhat deprecated in favor of frame-src and worker-src.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder ChildSources => Custom("child-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for form actions. Restricts the URLs which can be used as targets of form submissions.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder FormActions => Custom("form-action").ForSources(_nonce);

    /// <summary>
    /// Set the policy for navigation. Restricts the URLs to which a document can initiate navigation.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder NavigateTo => Custom("navigate-to").ForSources(_nonce);

    /// <summary>
    /// Set the policy for prefetch sources. Controls which URLs can be prefetched or prerendered.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspSourceDirectiveBuilder PrefetchSources => Custom("prefetch-src").ForSources(_nonce);

    /// <summary>
    /// Set the policy for require-trusted-types-for. Requires Trusted Types in specified contexts to prevent DOM-based XSS attacks.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder RequireTrustedTypesFor => Custom("require-trusted-types-for");

    /// <summary>
    /// Set the policy for trusted-types. Defines naming schema and behavior for Trusted Types policies.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder TrustedTypes => Custom("trusted-types");

    /// <summary>
    /// Enable upgrade-insecure-requests directive. Instructs browsers to treat all HTTP URLs as if they had been replaced with HTTPS.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspOptionsBuilder EnableUpgradeInsecureRequests()
    {
        Custom("upgrade-insecure-requests");
        return this;
    }

    /// <summary>
    /// Set the sandbox directive. Restricts actions that the page can take (similar to iframe sandbox attribute).
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder Sandbox => Custom("sandbox");

    /// <summary>
    /// Set the report-to directive. Modern replacement for report-uri that specifies a reporting group
    /// to send reports to.
    /// </summary>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder ReportTo => Custom("report-to");

    private CspDirectiveBuilder ReportUri => Custom("report-uri");

    /// <summary>
    /// Set a custom directive that is not listed as a property of this builder.
    /// </summary>
    /// <param name="directiveName">The name of the custom directive.</param>
    /// <returns>The builder for chaining.</returns>
    public CspDirectiveBuilder Custom(string directiveName)
    {
        directiveName.EnsureNotNull();

        return Maybe.From(_directiveBuilders.Find(d => d.Name == directiveName))
            .Unwrap(() =>
            {
                var newBuilder = new CspDirectiveBuilder(directiveName);
                _directiveBuilders.Add(newBuilder);
                return newBuilder;
            });
    }

    // Remove obsolete code in next major version
#if NET8_0
#pragma warning disable S1133
#endif

    /// <summary>
    /// Set the header to report-only. Content will not be blocked.
    /// Report-uri is deprecated, but the report-to directive is not yet supported in most browsers.
    /// </summary>
    /// <param name="reportUri">The optional URI to send JSON reports to of content that would be normally blocked.</param>
    /// <returns>The builder for chaining.</returns>
    [Obsolete("Report-uri is deprecated, but the report-to directive is not yet supported in most browsers. See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/report-uri")]
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
    [Obsolete("Report-uri is deprecated, but the report-to directive is not yet supported in most browsers. See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy/report-uri")]
    public CspOptionsBuilder SetReportUri(string reportUri)
    {
        if (ReportUri.Sources.Count > 0)
        {
            throw new InvalidOperationException("Cannot set more than one report-uri directive value.");
        }

        ReportUri.Allow(reportUri);
        return this;
    }

#if NET8_0
#pragma warning restore S1133
#endif

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
