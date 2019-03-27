using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Middleware for adding CSP security headers to HTTP responses.
    /// See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
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
        public async Task Invoke(HttpContext context)
        {
            var reportOnly = _options.IsReportOnly ? "-Report-Only" : string.Empty;
            context.Response.Headers.Add($"Content-Security-Policy{reportOnly}", GetHeaderValue());
            await _next(context);
        }

        private string GetHeaderValue()
        {
            var stringBuilder = new StringBuilder();

            var directiveBuilders = _options.DirectiveBuilders;

            foreach (var directiveBuilder in directiveBuilders)
            {
                stringBuilder.Append(directiveBuilder.Build());
            }

            return stringBuilder.ToString();
        }
    }
}
