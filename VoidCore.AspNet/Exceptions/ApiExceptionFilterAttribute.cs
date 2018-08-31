using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Exceptions
{
    /// <summary>
    /// A filter that handles exceptions for API routes by logging and sending a data message.
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Construct a new ApiExceptionFilterAttribute
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HttpContext</param>
        /// <param name="logger">Logging service</param>
        public ApiExceptionFilterAttribute(IHttpContextAccessor httpContextAccessor, ILoggingService logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var requestPath = context.HttpContext.Request.Path.ToString();

            if (!requestPath.StartsWith(ApiRoute.BasePath))
            {
                return;
            }

            _logger.Error(context.Exception);
            var errorMessage = new ErrorUserMessage("There was a problem processing your request.");
            context.Result = new ObjectResult(errorMessage) { StatusCode = 500 };
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggingService _logger;
    }
}
