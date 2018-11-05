using Moq;
using VoidCore.Model.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class MessagesEventLoggerTests
    {
        [Fact]
        public void LogPostSuccessUserMessageInt()
        {
            var result = Result.Ok(PostSuccessUserMessage.Create("Good stuff happened", 7));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new PostSuccessUserMessageEventLogger<string, int>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Message: Good stuff happened", "EntityId: 7"), Times.Once());
        }

        [Fact]
        public void LogPostSuccessUserMessageString()
        {
            var result = Result.Ok(PostSuccessUserMessage.Create("Good stuff happened", "7"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new PostSuccessUserMessageEventLogger<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Message: Good stuff happened", "EntityId: 7"), Times.Once());
        }

        [Fact]
        public void LogUserMessage()
        {
            var result = Result.Ok(new UserMessage("good"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new UserMessageEventLogger<string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Message: good"), Times.Once());
        }
    }
}