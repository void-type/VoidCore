using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using VoidCore.Model.Responses.Messages;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// A filter that handles exceptions for API routes with a UserMessage and 500 status code.
/// In development, the message is the exception. Otherwise, will return a generic message.
/// The exception is always logged.
/// API status codes are ignored and returned verbatim, even if empty.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public partial class ApiRouteExceptionFilterAttribute : ExceptionFilterAttribute
{
    private const string Message = "There was a problem processing your request.";
    private readonly ILogger _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Construct a new ApiExceptionFilterAttribute
    /// </summary>
    /// <param name="logger">A Logging service</param>
    /// <param name="environment">Environment</param>
    public ApiRouteExceptionFilterAttribute(ILogger<ApiRouteExceptionFilterAttribute> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <inheritdoc/>
    public override void OnException(ExceptionContext context)
    {
        var isApiRequest = ApiRouteAttribute.IsApiRequest(context.HttpContext);

        if (!isApiRequest)
        {
            return;
        }

        LogException(context.Exception);

        var userMessage = _environment.IsDevelopment() ? context.Exception.ToString() : Message;

        context.Result = new ObjectResult(new UserMessage(userMessage)) { StatusCode = 500 };
    }

    [LoggerMessage(0, LogLevel.Error, Message)]
    private partial void LogException(Exception ex);
}
