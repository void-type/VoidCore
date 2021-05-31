using Microsoft.AspNetCore.Http;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Extension methods for HttpContext.
    /// Adopted from https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
    /// </summary>
    public static class HttpContextExtensions
    {
        private static readonly string _nonceKey = "VoidCore_CSP_Nonce";

        /// <summary>
        /// Set the nonce on the HttpContext.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="nonce">The nonce</param>
        public static void SetNonce(this HttpContext context, string nonce)
        {
            context.Items[_nonceKey].Ensure(
                x => x is null, nameof(_nonceKey),
                "Nonce was already set on the HttpContext. Possible conflicting security policies defined.");

            context.Items[_nonceKey] = nonce;
        }

        /// <summary>
        /// Get the nonce from the HttpContext.
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns></returns>
        public static string GetNonce(this HttpContext context)
        {
            var nonce = context.Items[_nonceKey] as string;
            return nonce.EnsureNotNullOrEmpty(_nonceKey, "Nonce was not found in HttpContext.");
        }
    }
}
