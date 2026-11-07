using Microsoft.Extensions.Logging;

namespace VoidCore.AspNet.Logging;

internal static partial class AspNetLogMessages
{
    private static readonly Func<ILogger, string, string, string?, IDisposable?> _requestScope =
        LoggerMessage.DefineScope<string, string, string?>("{TraceId}:{RequestMethod}:{RequestPath}");

    public static IDisposable? BeginRequestScope(ILogger logger, string traceId, string requestMethod, string? requestPath)
    {
        return _requestScope(logger, traceId, requestMethod, requestPath);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with {ResponseType}. AppName: {ApplicationName} UserName: {UserName} UserAuthorizedAs: {AuthorizedAs}")]
    public static partial void RespondedWithWebClientInfo(ILogger logger, string responseType, string applicationName, string userName, string authorizedAs);
}
