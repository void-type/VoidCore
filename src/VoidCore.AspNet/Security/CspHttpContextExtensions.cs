using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Extension methods for HttpContext.
/// Adopted from https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
/// </summary>
public static class CspHttpContextExtensions
{
    private const string NonceKey = "VoidCore_CSP_Nonce";

    /// <summary>
    /// Get the nonce from the HttpContext. Sets the nonce if one hasn't been set yet.
    /// </summary>
    /// <param name="context">The context</param>
    public static string GetNonce(this HttpContext context)
    {
        var nonce = context.Items[NonceKey] as string;

        if (string.IsNullOrWhiteSpace(nonce))
        {
            context.Items[NonceKey] = nonce = GenerateNonce();
        }

        return nonce;
    }

    /// <summary>
    /// Utility method to generate a new random nonce.
    /// </summary>
    public static string GenerateNonce()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}
