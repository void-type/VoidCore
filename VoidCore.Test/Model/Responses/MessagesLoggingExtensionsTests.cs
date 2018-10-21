using Moq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class MessagesLoggingExtensionsTests
    {
        [Fact]
        public void PostSuccessUserMessageGetLogTextWithIntId()
        {
            var logText = PostSuccessUserMessage.Create("Hi.", 2).GetLogText();
            var expected = new [] { "Message: Hi.", "EntityId: 2" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void PostSuccessUserMessageGetLogTextWithStringId()
        {
            var logText = PostSuccessUserMessage.Create("Hi.", "ekdki23890lsdkalk").GetLogText();
            var expected = new [] { "Message: Hi.", "EntityId: ekdki23890lsdkalk" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void SuccessUserMessageGetLogText()
        {
            var logText = new UserMessage("Hi.").GetLogText();
            var expected = new [] { "Message: Hi." };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void LogPostSuccessUserMessage()
        {
            var result = Result.Ok(PostSuccessUserMessage.Create("good", "1"));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new PostSuccessUserMessageEventLogger<string, string>(loggerMock.Object);

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

            var processor = new UserMessageEventLogger<string>(loggerMock.Object);

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

            var processor = new SimpleFileEventLogger<string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("FileName: good"), Times.Once());
        }
    }
}
