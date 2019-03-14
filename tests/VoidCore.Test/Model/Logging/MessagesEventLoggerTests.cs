using Moq;
using VoidCore.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class MessagesEventLoggerTests
    {
        [Fact]
        public void LogUserMessageWithEntityIdInteger()
        {
            var result = Result.Ok(UserMessageWithEntityId.Create("Good stuff happened", 7));

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new UserMessageWithEntityIdEventLogger<string, int>(loggerMock.Object);

            processor.Process(string.Empty, result);

            loggerMock.Verify(l => l.Info("Message: Good stuff happened", "EntityId: 7"), Times.Once());
        }

        [Fact]
        public void LogUserMessageWithEntityIdString()
        {
            var result = Result.Ok(UserMessageWithEntityId.Create("Good stuff happened", "7"));

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new UserMessageWithEntityIdEventLogger<string, string>(loggerMock.Object);

            processor.Process(string.Empty, result);

            loggerMock.Verify(l => l.Info("Message: Good stuff happened", "EntityId: 7"), Times.Once());
        }

        [Fact]
        public void LogUserMessage()
        {
            var result = Result.Ok(new UserMessage("good"));

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new UserMessageEventLogger<string>(loggerMock.Object);

            processor.Process(string.Empty, result);

            loggerMock.Verify(l => l.Info("Message: good"), Times.Once());
        }
    }
}
