using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Middleware for adding CSP security headers to HTTP responses.
    /// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
    /// </summary>
    public sealed class CspMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CspOptions _options;

        /// <summary>
        /// Construct a new CspMiddleware.
        /// </summary>
        /// <param name="next">The next RequestDelegate</param>
        /// <param name="options">The options for configuring the header.</param>
        public CspMiddleware(RequestDelegate next, CspOptions options)
        {
            _next = next;
            _options = options;
        }

        /// <summary>
        /// Invoke the middleware.
        /// </summary>
        /// <param name="context">The current HttpContext</param>
        public Task Invoke(HttpContext context)
        {
            context.EnsureNotNull(nameof(context));

            var header = new CspHeader(_options);
            context.Response.Headers.Add(header.Key, header.Value);
            return _next(context);
        }
    }
}
