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

    // The source generator can't make generic log methods, so this mirrors the generated code with a delegate cached per id type.
    // That avoids boxing value-type ids. The event ID is the one the generator used for this message.
    public static void RespondedWithEntityMessage<TId>(ILogger logger, string message, TId entityId)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            EntityMessageLog<TId>.Responded(logger, message, entityId, null);
        }
    }

    private static class EntityMessageLog<TId>
    {
        public static readonly Action<ILogger, string, TId, Exception?> Responded = LoggerMessage.Define<string, TId>(
            LogLevel.Information,
            new EventId(421252945, nameof(RespondedWithEntityMessage)),
            "Responded with EntityMessage. Message: {Message} EntityId: {EntityId}",
            new LogDefineOptions { SkipEnabledCheck = true });
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Responded with SimpleFile. FileName: {FileName} FileSizeBytes: {FileSizeBytes} bytes")]
    public static partial void RespondedWithSimpleFile(ILogger logger, string fileName, int fileSizeBytes);
}
