using Microsoft.AspNetCore.Http;
using System;
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
    /// Get the nonce from the HttpContext.
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
    /// Get a new Nonce
    /// </summary>
    public static string GenerateNonce()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}
