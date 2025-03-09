using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Tag helper that adds a CSP nonce attribute to script and style tags.
/// </summary>
[HtmlTargetElement("script", Attributes = TagAttributeName)]
[HtmlTargetElement("style", Attributes = TagAttributeName)]
[HtmlTargetElement("link", Attributes = TagAttributeName)]
public class CspNonceTagHelper : TagHelper
{
    private const string NonceAttributeName = "nonce";
    private const string TagAttributeName = "voidcore-csp-nonce";

    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructs a new CspNonceTagHelper.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor</param>
    public CspNonceTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        // Get nonce from the HttpContext using the extension method
        var nonce = _httpContextAccessor.HttpContext.GetNonce();

        // Add the nonce attribute to the tag
        output.Attributes.SetAttribute(NonceAttributeName, nonce);

        // Remove our custom attribute
        output.Attributes.RemoveAll(TagAttributeName);
    }
}
