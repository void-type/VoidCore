using Moq;
using System.Collections.Generic;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.File;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Message;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class LoggingPostProcessorTests
    {
        [Fact]
        public void LogFallible()
        {
            var result = Result.Fail<string>("oops");

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Warn(It.IsAny<string[]>()));

            var processor = new FallibleLogging<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Warn(new[] {"Count: 1", "Failures: oops"}), Times.Once());
        }

        [Fact]
        public void LogItemSet()
        {
            var result = Result.Ok(new List<string>() { "one", "two" }.ToItemSet());

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new ItemSetLogging<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Count: 2"), Times.Once());
        }

        [Fact]
        public void LogItemSetPage()
        {
            var result = Result.Ok(new List<string>() { "one", "two" }.ToItemSetPage(1, 1));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new ItemSetPageLogging<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Count: 1", "Page: 1", "Take: 1", "TotalCount: 2"), Times.Once());
        }

        [Fact]
        public void LogPostSuccessUserMessage()
        {
            var result = Result.Ok(PostSuccessUserMessage.Create("good", "1"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new PostSuccessUserMessageLogging<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Message: good", "EntityId: 1"), Times.Once());
        }

        [Fact]
        public void LogUserMessage()
        {
            var result = Result.Ok(new UserMessage("good"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new UserMessageLogging<string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Message: good"), Times.Once());
        }

        [Fact]
        public void LogSimpleFile()
        {
            var result = Result.Ok(new SimpleFile("stuff inside", "good"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new SimpleFileLogging<string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("FileName: good"), Times.Once());
        }
    }
}
