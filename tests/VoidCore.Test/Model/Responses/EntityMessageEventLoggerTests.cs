using Microsoft.Extensions.Logging;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Responses;

public class EntityMessageEventLoggerTests
{
    [Fact]
    public void Logs_the_message_and_id()
    {
        var logger = new CapturingLogger<EntityMessageEventLogger<string, int>>();
        var eventLogger = new EntityMessageEventLogger<string, int>(logger);

        eventLogger.Process("request", Result.Ok(EntityMessage.Create("Saved.", 42)));

        var entry = Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Information, entry.Level);
        Assert.Equal("Responded with EntityMessage. Message: Saved. EntityId: 42", entry.Message);
        Assert.Equal("RespondedWithEntityMessage", entry.EventId.Name);
    }

    [Fact]
    public void Skips_the_log_when_information_is_disabled()
    {
        var logger = new CapturingLogger<EntityMessageEventLogger<string, int>>(LogLevel.Warning);
        var eventLogger = new EntityMessageEventLogger<string, int>(logger);

        eventLogger.Process("request", Result.Ok(EntityMessage.Create("Saved.", 42)));

        Assert.Empty(logger.Entries);
    }

    private sealed class CapturingLogger<T> : ILogger<T>
    {
        private readonly LogLevel _minimumLevel;

        public CapturingLogger(LogLevel minimumLevel = LogLevel.Trace)
        {
            _minimumLevel = minimumLevel;
        }

        public List<(LogLevel Level, EventId EventId, string Message)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minimumLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, eventId, formatter(state, exception)));
        }
    }
}
