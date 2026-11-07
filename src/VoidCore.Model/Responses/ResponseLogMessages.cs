using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses;

internal static partial class ResponseLogMessages
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Count: {Count} Failures: {FailureMessages}")]
    public static partial void Failures(ILogger logger, int count, string failureMessages);

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with ItemSet. Count: {Count} IsPagingEnabled: {IsPagingEnabled} Page: {Page} Take: {Take} TotalCount: {TotalCount}")]
    public static partial void RespondedWithItemSet(ILogger logger, int count, bool isPagingEnabled, int page, int take, int totalCount);

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with UserMessage. Message: {Message}")]
    public static partial void RespondedWithUserMessage(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with EntityMessage. Message: {Message} EntityId: {EntityId}")]
    public static partial void RespondedWithEntityMessage(ILogger logger, string message, object? entityId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with SimpleFile. FileName: {FileName} FileSizeBytes: {FileSizeBytes} bytes")]
    public static partial void RespondedWithSimpleFile(ILogger logger, string fileName, int fileSizeBytes);
}
