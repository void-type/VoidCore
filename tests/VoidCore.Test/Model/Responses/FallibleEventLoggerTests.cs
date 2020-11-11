using Moq;
using VoidCore.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class FallibleEventLoggerTests
    {
        [Fact]
        public void Failures_can_be_logged()
        {
            var result = Result.Fail<string>(
                new Failure("oops1", "uiHandle"),
                new Failure("oops2", "uiHandle"));

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Warn(It.IsAny<string[]>()));

            var processor = new FallibleEventLogger<string, string>(loggerMock.Object);

            processor.Process(string.Empty, result);

            loggerMock.Verify(l => l.Warn("Count: 2", "Failures: oops1 oops2"), Times.Once());
        }
    }
}
