using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace VoidCore.AspNet.Attributes
{
    /// <summary>
    /// A filter that handles exceptions for API routes by logging and sending a data message.
    /// </summary>
    public class ApiRouteExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Construct a new ApiExceptionFilterAttribute
        /// </summary>
        /// <param name="logger">Logging service</param>
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

        private readonly ILoggingService _logger;
    }
}
