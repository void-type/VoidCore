using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace VoidCore.AspNet.Routing
{
    /// <summary>
    /// A filter that handles exceptions for API routes by logging and responding with a user message.
    /// </summary>
    public class ApiRouteExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILoggingService _logger;

        /// <summary>
        /// Construct a new ApiExceptionFilterAttribute
        /// </summary>
        /// <param name="logger">A Logging service</param>
        public ApiRouteExceptionFilterAttribute(ILoggingService logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public override void OnException(ExceptionContext context)
        {
            var isApiRequest = context.HttpContext.Request.Path
                .StartsWithSegments(ApiRouteAttribute.BasePath, StringComparison.OrdinalIgnoreCase);

            if (!isApiRequest)
            {
                return;
            }

            const string message = "There was a problem processing your request.";
            _logger.Fatal(context.Exception, message);
            context.Result = new ObjectResult(new UserMessage(message)) { StatusCode = 500 };
        }
    }
}
