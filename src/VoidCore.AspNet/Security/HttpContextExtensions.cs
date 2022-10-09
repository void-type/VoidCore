using Microsoft.AspNetCore.Http;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Extension methods for HttpContext.
/// Adopted from https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
/// </summary>
public static class HttpContextExtensions
{
    private const string NonceKey = "VoidCore_CSP_Nonce";

    /// <summary>
    /// Set the nonce on the HttpContext.
    /// </summary>
    /// <param name="context">The context</param>
    /// <param name="nonce">The nonce</param>
    public static void SetNonce(this HttpContext context, string nonce)
    {
        context.Items[NonceKey].Ensure(
            x => x is null,
            "Nonce was already set on the HttpContext. Possible conflicting security policies defined.");

        context.Items[NonceKey] = nonce;
    }

    /// <summary>
    /// Get the nonce from the HttpContext.
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns></returns>
    public static string GetNonce(this HttpContext context)
    {
        var nonce = context.Items[NonceKey] as string;
        return nonce.EnsureNotNullOrEmpty("Nonce was not found in HttpContext.");
    }
}
